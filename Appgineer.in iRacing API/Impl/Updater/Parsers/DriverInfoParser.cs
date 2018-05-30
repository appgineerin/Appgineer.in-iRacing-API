// -----------------------------------------------------
//
// Distributed under GNU GPLv3.
//
// -----------------------------------------------------
//
// Copyright (c) 2018, appgineering.com
// All rights reserved.
// 
// This file is part of the Appgineer.in iRacing API.
//
// -----------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using AiRAPI.Data.Entity;
using AiRAPI.Data.Enums;
using AiRAPI.Data.Session;
using AiRAPI.Impl.Entity;
using AiRAPI.Impl.Session;
using AiRAPI.Impl.Utils;
using YamlDotNet.RepresentationModel;

namespace AiRAPI.Impl.Updater.Parsers
{
    internal sealed class DriverInfoParser : Parser
    {
        private readonly DataUpdater _updater;

        internal DriverInfoParser(DataUpdater updater)
        {
            _updater = updater;
        }

        internal override void Parse(YamlMappingNode root, Simulation sim)
        {
            var entities = root.GetList("DriverInfo.Drivers");
            var newEntities = sim.Session.Info.IsTeamRacing ? ParseTeamSession(entities, sim) : ParseSingleSession(entities, sim);
            if (newEntities)
                RecalcStrengthOfField(sim);
        }

        private bool ParseTeamSession(YamlSequenceNode entities, Simulation sim)
        {
            var newEntities = false;
            foreach (var entity in entities.Children.OfType<YamlMappingNode>())
            {
                var carIdx = entity.GetByte("CarIdx");
                var userId = entity.GetInt("UserID");
                if (sim.Session.Entities.SingleOrDefault(t => t.CarIdx == carIdx) is Team team)
                {
                    newEntities |= CheckDriverSwap(entity, sim, team, carIdx, userId);
                    continue;
                }

                if (userId <= 0 || userId >= int.MaxValue)
                    continue;

                if (entity.GetBool("IsSpectator"))
                    continue;

                if (entity.GetBool("CarIsPaceCar"))
                    continue;
                
                lock (sim.SharedCollectionLock)
                {
                    ((Session.Session) sim.Session).EntitiesInt.Add(CreateTeam(entity, sim, carIdx, userId));
                }
                newEntities = true;
            }

            return newEntities;
        }

        private bool CheckDriverSwap(YamlMappingNode entity, Simulation sim, Team team, byte carIdx, int userId)
        {
            var newEntity = false;
            if (team.CurrentDriver != null && team.CurrentDriver.Id == userId)
                return false;

            if (!(team.Drivers.SingleOrDefault(d => d.Id == userId) is Driver driver))
            {
                lock (sim.SharedCollectionLock)
                {
                    driver = CreateDriver(entity, sim, carIdx, userId, team.Suit, team.Car);
                    lock (sim.SharedCollectionLock)
                    {
                        team.DriversInt.Add(driver);
                    }
                }
                
                newEntity = true;
            }

            if (team.CurrentDriver != null)
                ((Driver)team.CurrentDriver).IsDriving = false;

            driver.IsDriving = true;
            team.CurrentDriver = driver;

            // TODO: SessionType / lapNumber are invalid.
            var sessionEvent = new SessionEvent((long)(sim.Telemetry.SessionTime * 60 + _updater.TimeOffset),
                "Driverswap for " + team.Name, team, sim.CameraManager.CurrentGroup, SessionType.None, SessionEventType.DriverSwap, 0);

            lock (sim.SharedCollectionLock)
            {
                ((Session.Session)sim.Session).SessionEventsInt.Add(sessionEvent);
            }

            return newEntity;
        }

        private static Team CreateTeam(YamlMappingNode entity, Simulation sim, byte carIdx, int userId)
        {
            var team = new Team
            {
                Name = entity.GetString("TeamName"),
                Id = entity.GetInt("TeamID"),
                CarIdx = carIdx,
                Car = CreateCar(entity, sim)
            };
            // TODO: Results.
            // TODO: Suit.

            var suitDesign = entity.GetString("SuitDesignStr").Split(',', ';').ToArray();
            if (suitDesign.Length == 4)
            {
                // ReSharper disable PossibleNullReferenceException
                team.SuitDesignNumber = int.Parse(suitDesign[0]);
                team.SuitColor1 = (Color)ColorConverter.ConvertFromString($"#{suitDesign[1]}");
                team.SuitColor2 = (Color)ColorConverter.ConvertFromString($"#{suitDesign[2]}");
                team.SuitColor2 = (Color)ColorConverter.ConvertFromString($"#{suitDesign[3]}");
                // ReSharper restore PossibleNullReferenceException
            }
            
            lock (sim.SharedCollectionLock)
            {
                team.DriversInt.Add(team.CurrentDriver = CreateDriver(entity, sim, carIdx, userId, team.Suit, team.Car));
            }
            return team;
        }

        private static bool ParseSingleSession(YamlSequenceNode entities, Simulation sim)
        {
            var newEntities = false;
            foreach (var entity in entities.Children.OfType<YamlMappingNode>())
            {
                var carIdx = entity.GetByte("CarIdx");
                var userId = entity.GetInt("UserID");

                if (sim.Session.Entities.SingleOrDefault(e => e.CarIdx == carIdx) is Driver)
                    continue;

                if (entity.GetBool("IsSpectator"))
                    continue;

                if (entity.GetBool("CarIsPaceCar"))
                    continue;
                
                lock (sim.SharedCollectionLock)
                {
                    ((Session.Session)sim.Session).EntitiesInt.Add(CreateDriver(entity, sim, carIdx, userId));
                }
                newEntities = true;
            }

            return newEntities;
        }

        private static Driver CreateDriver(YamlMappingNode entity, Simulation sim, byte carIdx, int userId, BitmapSource suit = null, ICar car = null)
        {
            var driver = new Driver
            {
                Name = entity.GetString("UserName"),
                Id = userId,
                CarIdx = carIdx,
                Car = car ?? CreateCar(entity, sim),
                IsDriving = true,
                Initials = entity.GetString("Initials"),
                ShortName = ParseShortName(entity.GetString("AbbrevName")),
                Club = ClubManager.GetClub(entity.GetString("ClubName")),
                LastName = entity.GetString("AbbrevName").Split(',').FirstOrDefault()?.Trim()
            };

            driver.DriversInt.Add(driver);
            driver.CurrentDriver = driver;
            // TODO: Results.
            // TODO: Suit.
            // TODO: Helmet.

            var suitDesign = entity.GetString("SuitDesignStr").Split(',', ';').ToArray();
            if (suitDesign.Length == 4)
            {
                // ReSharper disable PossibleNullReferenceException
                driver.SuitDesignNumber = int.Parse(suitDesign[0]);
                driver.SuitColor1 = (Color)ColorConverter.ConvertFromString($"#{suitDesign[1]}");
                driver.SuitColor2 = (Color)ColorConverter.ConvertFromString($"#{suitDesign[2]}");
                driver.SuitColor2 = (Color)ColorConverter.ConvertFromString($"#{suitDesign[3]}");
                // ReSharper restore PossibleNullReferenceException
            }

            var helmetDesign = entity.GetString("HelmetDesignStr").Split(',', ';').ToArray();
            if (helmetDesign.Length == 4)
            {
                // ReSharper disable PossibleNullReferenceException
                driver.HelmetDesignNumber = int.Parse(helmetDesign[0]);
                driver.HelmetColor1 = (Color)ColorConverter.ConvertFromString($"#{helmetDesign[1]}");
                driver.HelmetColor2 = (Color)ColorConverter.ConvertFromString($"#{helmetDesign[2]}");
                driver.HelmetColor2 = (Color)ColorConverter.ConvertFromString($"#{helmetDesign[3]}");
                // ReSharper restore PossibleNullReferenceException
            }
            
            object convertFromString = null;
            try
            {
                var colorString = "#FF" + entity.GetString("LicColor").Substring(2);
                convertFromString = ColorConverter.ConvertFromString(colorString);
            }
            catch
            {
                // ignore
            }

            convertFromString = convertFromString ?? Colors.White;
            driver.License = new License(entity.GetInt("LicLevel"), entity.GetInt("LicSubLevel"), (Color)convertFromString, entity.GetInt("IRating"));

            if (string.IsNullOrWhiteSpace(driver.LastName))
                driver.ThreeLetterCode = string.Empty;
            else if (driver.LastName.Length < 2)
                driver.ThreeLetterCode = (driver.Initials + driver.LastName[0]).ToUpper();
            else
                driver.ThreeLetterCode = (driver.Initials + driver.LastName[1]).ToUpper();

            driver.Division = entity.GetString("DivisionName");
            return driver;
        }

        private static ICar CreateCar(YamlMappingNode entity, Simulation sim)
        {
            var car = new Car
            {
                Number = entity.GetString("CarNumber").Replace("\"", string.Empty),
                Path = entity.GetString("CarPath"),
                Name = entity.GetString("CarScreenName"),
                Id = entity.GetInt("CarID"),
                Class = sim.Session.ClassManager.GetClass(entity.GetString("CarClassShortName")) ?? CreateClass(entity, sim)
            };

            car.NumberPadded = car.Number.PadCarNum();

            // TODO: Image

            var carDesign = entity.GetString("CarDesignStr").Split(',', ';').ToArray();
            if (carDesign.Length >= 4)
            {
                // ReSharper disable PossibleNullReferenceException
                car.DesignNumber = int.Parse(carDesign[0]);
                car.Color1 = (Color)ColorConverter.ConvertFromString($"#{carDesign[1]}");
                car.Color2 = (Color)ColorConverter.ConvertFromString($"#{carDesign[2]}");
                car.Color3 = (Color)ColorConverter.ConvertFromString($"#{carDesign[3]}");
                if (carDesign.Length == 5)
                    car.Color4 = (Color)ColorConverter.ConvertFromString($"#{carDesign[4]}");
                // ReSharper restore PossibleNullReferenceException
            }

            var carNumberDesign = entity.GetString("CarNumberDesignStr").Split(',', ';').ToArray();
            // ReSharper disable once InvertIf
            if (carNumberDesign.Length == 5)
            {
                // ReSharper disable PossibleNullReferenceException
                car.NumberDesignNumber1 = int.Parse(carNumberDesign[0]);
                car.NumberDesignNumber2 = int.Parse(carNumberDesign[1]);
                car.NumberColor1 = (Color)ColorConverter.ConvertFromString($"#{carNumberDesign[2]}");
                car.NumberColor2 = (Color)ColorConverter.ConvertFromString($"#{carNumberDesign[3]}");
                car.NumberColor3 = (Color)ColorConverter.ConvertFromString($"#{carNumberDesign[4]}");
                // ReSharper restore PossibleNullReferenceException
            }

            return car;
        }

        private static IClass CreateClass(YamlMappingNode entity, ISimulation sim)
        {
            var color = entity.GetString("CarClassColor");
            object convertFromString = null;
            if (!string.IsNullOrWhiteSpace(color) && color.Length > 2)
                convertFromString = ColorConverter.ConvertFromString("#FF" + entity.GetString("CarClassColor").Substring(2));

            var clazz = new Class
            {
                Id = entity.GetByte("CarClassID"),
                RelativeSpeed = entity.GetInt("CarClassRelSpeed"),
                Name = entity.GetString("CarClassShortName"),
                Color = (Color?)convertFromString ?? Colors.White
            };

            lock (sim.SharedCollectionLock)
                ((ClassManager)sim.Session.ClassManager).AddClass(clazz);

            return clazz;
        }

        private static void RecalcStrengthOfField(ISimulation sim)
        {
            var ln = 1600 / Math.Log(2);
            var sofExp = (from entity in sim.Session.Entities from driver in entity.Drivers select Math.Exp(-driver.License.IRating/ln)).ToList();

            if (!(sim.Session is Session.Session session))
                return;

            session.StrengthOfField = (int)Math.Round(ln * Math.Log(sofExp.Count / sofExp.Aggregate((curr, next) => curr + next)));
            foreach (var clazz in sim.Session.ClassManager.Classes.OfType<Class>())
                CalcStrengthOfClass(clazz, session, ln);
        }

        private static void CalcStrengthOfClass(IClass clazz, ISession session, double ln)
        {
            var sofExp = new List<double>();
            foreach (var entity in session.Entities.Where(e => e.Car.Class.Equals(clazz)))
                sofExp.AddRange(entity.Drivers.Select(driver => Math.Exp(-driver.License.IRating / ln)));
            
            clazz.StrengthOfClass = (int)Math.Round(ln * Math.Log(sofExp.Count / sofExp.Aggregate((curr, next) => curr + next)));
        }

        private static string ParseShortName(string abbrevName)
        {
            if (abbrevName == null)
                return "";

            var splitName = abbrevName.Split(',');
            return splitName.Length > 1 ? (splitName[1] + " " + splitName[0]).Trim() : abbrevName.Trim();
        }
    }
}
