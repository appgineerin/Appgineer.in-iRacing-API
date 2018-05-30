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

using System.Linq;
using System.Reflection;
using AiRAPI.Data;
using AiRAPI.Data.Enums;
using AiRAPI.MVVM;
using AiRAPI.SDK;

// ReSharper disable InconsistentNaming

namespace AiRAPI.Impl
{
    internal sealed class Telemetry : BindableBase, ITelemetry
    {
        private double _sessionTime;
        public double SessionTime
        {
            get => _sessionTime;
            internal set => SetProperty(ref _sessionTime, value);
        }

        private long _sessionTick;
        public long SessionTick
        {
            get => _sessionTick;
            internal set => SetProperty(ref _sessionTick, value);
        }

        private int _sessionNum;
        public int SessionNum
        {
            get => _sessionNum;
            internal set => SetProperty(ref _sessionNum, value);
        }

        private SessionState _sessionState;
        public SessionState SessionState
        {
            get => _sessionState;
            internal set => SetProperty(ref _sessionState, value);
        }

        private int _sessionUniqueID;
        public int SessionUniqueID
        {
            get => _sessionUniqueID;
            internal set => SetProperty(ref _sessionUniqueID, value);
        }

        private SessionFlags _sessionFlags;
        public SessionFlags SessionFlags
        {
            get => _sessionFlags;
            internal set => SetProperty(ref _sessionFlags, value);
        }

        private double _sessionTimeRemain;
        public double SessionTimeRemain
        {
            get => _sessionTimeRemain;
            internal set => SetProperty(ref _sessionTimeRemain, value);
        }

        private int _sessionLapsRemain;
        public int SessionLapsRemain
        {
            get => _sessionLapsRemain;
            internal set => SetProperty(ref _sessionLapsRemain, value);
        }

        private int _sessionLapsRemainEx;
        public int SessionLapsRemainEx
        {
            get => _sessionLapsRemainEx;
            internal set => SetProperty(ref _sessionLapsRemainEx, value);
        }

        private int _radioTransmitCarIdx;
        public int RadioTransmitCarIdx
        {
            get => _radioTransmitCarIdx;
            internal set => SetProperty(ref _radioTransmitCarIdx, value);
        }

        private int _radioTransmitRadioIdx;
        public int RadioTransmitRadioIdx
        {
            get => _radioTransmitRadioIdx;
            internal set => SetProperty(ref _radioTransmitRadioIdx, value);
        }

        private int _radioTransmitFrequencyIdx;
        public int RadioTransmitFrequencyIdx
        {
            get => _radioTransmitFrequencyIdx;
            internal set => SetProperty(ref _radioTransmitFrequencyIdx, value);
        }

        private int _displayUnits;
        public int DisplayUnits
        {
            get => _displayUnits;
            internal set => SetProperty(ref _displayUnits, value);
        }

        private bool _driverMarker;
        public bool DriverMarker
        {
            get => _driverMarker;
            internal set => SetProperty(ref _driverMarker, value);
        }

        private bool _isOnTrack;
        public bool IsOnTrack
        {
            get => _isOnTrack;
            internal set => SetProperty(ref _isOnTrack, value);
        }

        private bool _isReplayPlaying;
        public bool IsReplayPlaying
        {
            get => _isReplayPlaying;
            internal set => SetProperty(ref _isReplayPlaying, value);
        }

        private int _replayFrameNum;
        public int ReplayFrameNum
        {
            get => _replayFrameNum;
            internal set => SetProperty(ref _replayFrameNum, value);
        }

        private int _replayFrameNumEnd;
        public int ReplayFrameNumEnd
        {
            get => _replayFrameNumEnd;
            internal set => SetProperty(ref _replayFrameNumEnd, value);
        }

        private bool _isDiskLoggingEnabled;
        public bool IsDiskLoggingEnabled
        {
            get => _isDiskLoggingEnabled;
            internal set => SetProperty(ref _isDiskLoggingEnabled, value);
        }

        private bool _isDiskLoggingActive;
        public bool IsDiskLoggingActive
        {
            get => _isDiskLoggingActive;
            internal set => SetProperty(ref _isDiskLoggingActive, value);
        }

        private float _frameRate;
        public float FrameRate
        {
            get => _frameRate;
            internal set => SetProperty(ref _frameRate, value);
        }

        private float _cpuUsageBG;
        public float CpuUsageBG
        {
            get => _cpuUsageBG;
            internal set => SetProperty(ref _cpuUsageBG, value);
        }

        private int _playerCarPosition;
        public int PlayerCarPosition
        {
            get => _playerCarPosition;
            internal set => SetProperty(ref _playerCarPosition, value);
        }

        private int _playerCarClassPosition;
        public int PlayerCarClassPosition
        {
            get => _playerCarClassPosition;
            internal set => SetProperty(ref _playerCarClassPosition, value);
        }

        private TrackLocation _playerTrackSurface;
        public TrackLocation PlayerTrackSurface
        {
            get => _playerTrackSurface;
            internal set => SetProperty(ref _playerTrackSurface, value);
        }

        private int _playerCarIdx;
        public int PlayerCarIdx
        {
            get => _playerCarIdx;
            internal set => SetProperty(ref _playerCarIdx, value);
        }

        private int _playerCarTeamIncidentCount;
        public int PlayerCarTeamIncidentCount
        {
            get => _playerCarTeamIncidentCount;
            internal set => SetProperty(ref _playerCarTeamIncidentCount, value);
        }

        private int _playerCarMyIncidentCount;
        public int PlayerCarMyIncidentCount
        {
            get => _playerCarMyIncidentCount;
            internal set => SetProperty(ref _playerCarMyIncidentCount, value);
        }

        private int _playerCarDriverIncidentCount;
        public int PlayerCarDriverIncidentCount
        {
            get => _playerCarDriverIncidentCount;
            internal set => SetProperty(ref _playerCarDriverIncidentCount, value);
        }

        private int[] _carIdxLap;
        public int[] CarIdxLap
        {
            get => _carIdxLap;
            internal set => SetProperty(ref _carIdxLap, value);
        }

        private int[] _carIdxLapCompleted;
        public int[] CarIdxLapCompleted
        {
            get => _carIdxLapCompleted;
            internal set => SetProperty(ref _carIdxLapCompleted, value);
        }

        private float[] _carIdxLapDistPct;
        public float[] CarIdxLapDistPct
        {
            get => _carIdxLapDistPct;
            internal set => SetProperty(ref _carIdxLapDistPct, value);
        }

        private TrackLocation[] _carIdxTrackSurface;
        public TrackLocation[] CarIdxTrackSurface
        {
            get => _carIdxTrackSurface;
            internal set => SetProperty(ref _carIdxTrackSurface, value);
        }

        private bool[] _carIdxOnPitRoad;
        public bool[] CarIdxOnPitRoad
        {
            get => _carIdxOnPitRoad;
            internal set => SetProperty(ref _carIdxOnPitRoad, value);
        }

        private int[] _carIdxPosition;
        public int[] CarIdxPosition
        {
            get => _carIdxPosition;
            internal set => SetProperty(ref _carIdxPosition, value);
        }

        private int[] _carIdxClassPosition;
        public int[] CarIdxClassPosition
        {
            get => _carIdxClassPosition;
            internal set => SetProperty(ref _carIdxClassPosition, value);
        }

        private float[] _carIdxF2Time;
        public float[] CarIdxF2Time
        {
            get => _carIdxF2Time;
            internal set => SetProperty(ref _carIdxF2Time, value);
        }

        private float[] _carIdxEstTime;
        public float[] CarIdxEstTime
        {
            get => _carIdxEstTime;
            internal set => SetProperty(ref _carIdxEstTime, value);
        }

        private bool _onPitRoad;
        public bool OnPitRoad
        {
            get => _onPitRoad;
            internal set => SetProperty(ref _onPitRoad, value);
        }

        private float[] _carIdxSteer;
        public float[] CarIdxSteer
        {
            get => _carIdxSteer;
            internal set => SetProperty(ref _carIdxSteer, value);
        }

        private float[] _carIdxRPM;
        public float[] CarIdxRPM
        {
            get => _carIdxRPM;
            internal set => SetProperty(ref _carIdxRPM, value);
        }

        private int[] _carIdxGear;
        public int[] CarIdxGear
        {
            get => _carIdxGear;
            internal set => SetProperty(ref _carIdxGear, value);
        }

        private float _steeringWheelAngle;
        public float SteeringWheelAngle
        {
            get => _steeringWheelAngle;
            internal set => SetProperty(ref _steeringWheelAngle, value);
        }

        private float _throttle;
        public float Throttle
        {
            get => _throttle;
            internal set => SetProperty(ref _throttle, value);
        }

        private float _brake;
        public float Brake
        {
            get => _brake;
            internal set => SetProperty(ref _brake, value);
        }

        private float _clutch;
        public float Clutch
        {
            get => _clutch;
            internal set => SetProperty(ref _clutch, value);
        }

        private int _gear;
        public int Gear
        {
            get => _gear;
            internal set => SetProperty(ref _gear, value);
        }

        private float _rpm;
        public float RPM
        {
            get => _rpm;
            internal set => SetProperty(ref _rpm, value);
        }

        private int _lap;
        public int Lap
        {
            get => _lap;
            internal set => SetProperty(ref _lap, value);
        }

        private int _lapCompleted;
        public int LapCompleted
        {
            get => _lapCompleted;
            internal set => SetProperty(ref _lapCompleted, value);
        }

        private float _lapDist;
        public float LapDist
        {
            get => _lapDist;
            internal set => SetProperty(ref _lapDist, value);
        }

        private float _lapDistPct;
        public float LapDistPct
        {
            get => _lapDistPct;
            internal set => SetProperty(ref _lapDistPct, value);
        }

        private int _raceLaps;
        public int RaceLaps
        {
            get => _raceLaps;
            internal set => SetProperty(ref _raceLaps, value);
        }

        private int _lapBestLap;
        public int LapBestLap
        {
            get => _lapBestLap;
            internal set => SetProperty(ref _lapBestLap, value);
        }

        private float _lapBestLapTime;
        public float LapBestLapTime
        {
            get => _lapBestLapTime;
            internal set => SetProperty(ref _lapBestLapTime, value);
        }

        private float _lapLastLapTime;
        public float LapLastLapTime
        {
            get => _lapLastLapTime;
            internal set => SetProperty(ref _lapLastLapTime, value);
        }

        private float _lapCurrentLapTime;
        public float LapCurrentLapTime
        {
            get => _lapCurrentLapTime;
            internal set => SetProperty(ref _lapCurrentLapTime, value);
        }

        private int _lapLasNLapSeq;
        public int LapLasNLapSeq
        {
            get => _lapLasNLapSeq;
            internal set => SetProperty(ref _lapLasNLapSeq, value);
        }

        private float _lapLastNLapTime;
        public float LapLastNLapTime
        {
            get => _lapLastNLapTime;
            internal set => SetProperty(ref _lapLastNLapTime, value);
        }

        private int _lapBestBLapLap;
        public int LapBestBLapLap
        {
            get => _lapBestBLapLap;
            internal set => SetProperty(ref _lapBestBLapLap, value);
        }

        private float _lapBestNLapTime;
        public float LapBestNLapTime
        {
            get => _lapBestNLapTime;
            internal set => SetProperty(ref _lapBestNLapTime, value);
        }

        private float _lapDeltaToBestLap;
        public float LapDeltaToBestLap
        {
            get => _lapDeltaToBestLap;
            internal set => SetProperty(ref _lapDeltaToBestLap, value);
        }

        private float _lapDeltaToBestLap_DD;
        public float LapDeltaToBestLap_DD
        {
            get => _lapDeltaToBestLap_DD;
            internal set => SetProperty(ref _lapDeltaToBestLap_DD, value);
        }

        private bool _lapDeltaToBestLap_OK;
        public bool LapDeltaToBestLap_OK
        {
            get => _lapDeltaToBestLap_OK;
            internal set => SetProperty(ref _lapDeltaToBestLap_OK, value);
        }

        private float _lapDeltaToOptimalLap;
        public float LapDeltaToOptimalLap
        {
            get => _lapDeltaToOptimalLap;
            internal set => SetProperty(ref _lapDeltaToOptimalLap, value);
        }

        private float _lapDeltaToOptimalLap_DD;
        public float LapDeltaToOptimalLap_DD
        {
            get => _lapDeltaToOptimalLap_DD;
            internal set => SetProperty(ref _lapDeltaToOptimalLap_DD, value);
        }

        private bool _lapDeltaToOptimalLap_OK;
        public bool LapDeltaToOptimalLap_OK
        {
            get => _lapDeltaToOptimalLap_OK;
            internal set => SetProperty(ref _lapDeltaToOptimalLap_OK, value);
        }

        private float _lapDeltaToSessionBestLap;
        public float LapDeltaToSessionBestLap
        {
            get => _lapDeltaToSessionBestLap;
            internal set => SetProperty(ref _lapDeltaToSessionBestLap, value);
        }

        private float _lapDeltaToSessionBestLap_DD;
        public float LapDeltaToSessionBestLap_DD
        {
            get => _lapDeltaToSessionBestLap_DD;
            internal set => SetProperty(ref _lapDeltaToSessionBestLap_DD, value);
        }

        private bool _lapDeltaToSessionBestLap_OK;
        public bool LapDeltaToSessionBestLap_OK
        {
            get => _lapDeltaToSessionBestLap_OK;
            internal set => SetProperty(ref _lapDeltaToSessionBestLap_OK, value);
        }

        private float _lapDeltaToSessionOptimalLap;
        public float LapDeltaToSessionOptimalLap
        {
            get => _lapDeltaToSessionOptimalLap;
            internal set => SetProperty(ref _lapDeltaToSessionOptimalLap, value);
        }

        private float _lapDeltaToSessionOptimalLap_DD;
        public float LapDeltaToSessionOptimalLap_DD
        {
            get => _lapDeltaToSessionOptimalLap_DD;
            internal set => SetProperty(ref _lapDeltaToSessionOptimalLap_DD, value);
        }

        private bool _lapDeltaToSessionOptimalLap_OK;
        public bool LapDeltaToSessionOptimalLap_OK
        {
            get => _lapDeltaToSessionOptimalLap_OK;
            internal set => SetProperty(ref _lapDeltaToSessionOptimalLap_OK, value);
        }

        private float _lapDeltaToSessionLastLap;
        public float LapDeltaToSessionLastLap
        {
            get => _lapDeltaToSessionLastLap;
            internal set => SetProperty(ref _lapDeltaToSessionLastLap, value);
        }

        private float _lapDeltaToSessionLastLap_DD;
        public float LapDeltaToSessionLastLap_DD
        {
            get => _lapDeltaToSessionLastLap_DD;
            internal set => SetProperty(ref _lapDeltaToSessionLastLap_DD, value);
        }

        private bool _lapDeltaToSessionLastLap_OK;
        public bool LapDeltaToSessionLastLap_OK
        {
            get => _lapDeltaToSessionLastLap_OK;
            internal set => SetProperty(ref _lapDeltaToSessionLastLap_OK, value);
        }

        private float _longAccel;
        public float LongAccel
        {
            get => _longAccel;
            internal set => SetProperty(ref _longAccel, value);
        }

        private float _latAccel;
        public float LatAccel
        {
            get => _latAccel;
            internal set => SetProperty(ref _latAccel, value);
        }

        private float _verAccel;
        public float VertAccel
        {
            get => _verAccel;
            internal set => SetProperty(ref _verAccel, value);
        }

        private float _rollRate;
        public float RollRate
        {
            get => _rollRate;
            internal set => SetProperty(ref _rollRate, value);
        }

        private float _pitchRate;
        public float PitchRate
        {
            get => _pitchRate;
            internal set => SetProperty(ref _pitchRate, value);
        }

        private float _yawRate;
        public float YawRate
        {
            get => _yawRate;
            internal set => SetProperty(ref _yawRate, value);
        }

        private float _speed;
        public float Speed
        {
            get => _speed;
            internal set => SetProperty(ref _speed, value);
        }

        private float _velocityX;
        public float VelocityX
        {
            get => _velocityX;
            internal set => SetProperty(ref _velocityX, value);
        }

        private float _velocityY;
        public float VelocityY
        {
            get => _velocityY;
            internal set => SetProperty(ref _velocityY, value);
        }

        private float _velocityZ;
        public float VelocityZ
        {
            get => _velocityZ;
            internal set => SetProperty(ref _velocityZ, value);
        }

        private float _yaw;
        public float Yaw
        {
            get => _yaw;
            internal set => SetProperty(ref _yaw, value);
        }

        private float _yawNorth;
        public float YawNorth
        {
            get => _yawNorth;
            internal set => SetProperty(ref _yawNorth, value);
        }

        private float _pitch;
        public float Pitch
        {
            get => _pitch;
            internal set => SetProperty(ref _pitch, value);
        }

        private float _roll;
        public float Roll
        {
            get => _roll;
            internal set => SetProperty(ref _roll, value);
        }

        private int _enterExitReset;
        public int EnterExitReset
        {
            get => _enterExitReset;
            internal set => SetProperty(ref _enterExitReset, value);
        }

        private float _trackTemp;
        public float TrackTemp
        {
            get => _trackTemp;
            internal set => SetProperty(ref _trackTemp, value);
        }

        private float _trackTempCrew;
        public float TrackTempCrew
        {
            get => _trackTempCrew;
            internal set => SetProperty(ref _trackTempCrew, value);
        }

        private float _airTemp;
        public float AirTemp
        {
            get => _airTemp;
            internal set => SetProperty(ref _airTemp, value);
        }

        private int _weatherType;
        public int WeatherType
        {
            get => _weatherType;
            internal set => SetProperty(ref _weatherType, value);
        }

        private int _skies;
        public int Skies
        {
            get => _skies;
            internal set => SetProperty(ref _skies, value);
        }

        private float _airDensity;
        public float AirDensity
        {
            get => _airDensity;
            internal set => SetProperty(ref _airDensity, value);
        }

        private float _airPressure;
        public float AirPressure
        {
            get => _airPressure;
            internal set => SetProperty(ref _airPressure, value);
        }

        private float _windVel;
        public float WindVel
        {
            get => _windVel;
            internal set => SetProperty(ref _windVel, value);
        }

        private float _windDir;
        public float WindDir
        {
            get => _windDir;
            internal set => SetProperty(ref _windDir, value);
        }

        private float _relativeHumidity;
        public float RelativeHumidity
        {
            get => _relativeHumidity;
            internal set => SetProperty(ref _relativeHumidity, value);
        }

        private float _fogLevel;
        public float FogLevel
        {
            get => _fogLevel;
            internal set => SetProperty(ref _fogLevel, value);
        }

        private int _dcLapStatus;
        public int DCLapStatus
        {
            get => _dcLapStatus;
            internal set => SetProperty(ref _dcLapStatus, value);
        }

        private int _dcDriversSoFar;
        public int DCDriversSoFar
        {
            get => _dcDriversSoFar;
            internal set => SetProperty(ref _dcDriversSoFar, value);
        }

        private bool _okToReloadTextures;
        public bool OkToReloadTextures
        {
            get => _okToReloadTextures;
            internal set => SetProperty(ref _okToReloadTextures, value);
        }

        private float _pitRepairLeft;
        public float PitRepairLeft
        {
            get => _pitRepairLeft;
            internal set => SetProperty(ref _pitRepairLeft, value);
        }

        private float _pitOptRepairLeft;
        public float PitOptRepairLeft
        {
            get => _pitOptRepairLeft;
            internal set => SetProperty(ref _pitOptRepairLeft, value);
        }

        private int _camCarIdx;
        public int CamCarIdx
        {
            get => _camCarIdx;
            internal set => SetProperty(ref _camCarIdx, value);
        }

        private int _camCameraNumber;
        public int CamCameraNumber
        {
            get => _camCameraNumber;
            internal set => SetProperty(ref _camCameraNumber, value);
        }

        private int _camGroupNumber;
        public int CamGroupNumber
        {
            get => _camGroupNumber;
            internal set => SetProperty(ref _camGroupNumber, value);
        }

        private CameraState _camCameraState;
        public CameraState CamCameraState
        {
            get => _camCameraState;
            internal set => SetProperty(ref _camCameraState, value);
        }

        private bool _isOnTrackCar;
        public bool IsOnTrackCar
        {
            get => _isOnTrackCar;
            internal set => SetProperty(ref _isOnTrackCar, value);
        }

        private bool _isInGarage;
        public bool IsInGarage
        {
            get => _isInGarage;
            internal set => SetProperty(ref _isInGarage, value);
        }

        private float _steeringWheelTorque;
        public float SteeringWheelTorque
        {
            get => _steeringWheelTorque;
            internal set => SetProperty(ref _steeringWheelTorque, value);
        }

        private float _steeringWheelPctTorque;
        public float SteeringWheelPctTorque
        {
            get => _steeringWheelPctTorque;
            internal set => SetProperty(ref _steeringWheelPctTorque, value);
        }

        private float _steeringWheelPctTorqueSign;
        public float SteeringWheelPctTorqueSign
        {
            get => _steeringWheelPctTorqueSign;
            internal set => SetProperty(ref _steeringWheelPctTorqueSign, value);
        }

        private float _steeringWheelPctTorqueSignStops;
        public float SteeringWheelPctTorqueSignStops
        {
            get => _steeringWheelPctTorqueSignStops;
            internal set => SetProperty(ref _steeringWheelPctTorqueSignStops, value);
        }

        private float _steeringWheelPctDamper;
        public float SteeringWheelPctDamper
        {
            get => _steeringWheelPctDamper;
            internal set => SetProperty(ref _steeringWheelPctDamper, value);
        }

        private float _steeringWheelAngleMax;
        public float SteeringWheelAngleMax
        {
            get => _steeringWheelAngleMax;
            internal set => SetProperty(ref _steeringWheelAngleMax, value);
        }

        private float _shiftIndicatorPct;
        public float ShiftIndicatorPct
        {
            get => _shiftIndicatorPct;
            internal set => SetProperty(ref _shiftIndicatorPct, value);
        }

        private float _shiftPowerPct;
        public float ShiftPowerPct
        {
            get => _shiftPowerPct;
            internal set => SetProperty(ref _shiftPowerPct, value);
        }

        private float _shiftGrindRPM;
        public float ShiftGrindRPM
        {
            get => _shiftGrindRPM;
            internal set => SetProperty(ref _shiftGrindRPM, value);
        }

        private float _throttleRaw;
        public float ThrottleRaw
        {
            get => _throttleRaw;
            internal set => SetProperty(ref _throttleRaw, value);
        }

        private float _brakeRaw;
        public float BrakeRaw
        {
            get => _brakeRaw;
            internal set => SetProperty(ref _brakeRaw, value);
        }

        private float _steeringWheelPeakForceNm;
        public float SteeringWheelPeakForceNm
        {
            get => _steeringWheelPeakForceNm;
            internal set => SetProperty(ref _steeringWheelPeakForceNm, value);
        }

        private EngineWarnings _engineWarnings;
        public EngineWarnings EngineWarnings
        {
            get => _engineWarnings;
            internal set => SetProperty(ref _engineWarnings, value);
        }

        private float _fuelLevel;
        public float FuelLevel
        {
            get => _fuelLevel;
            internal set => SetProperty(ref _fuelLevel, value);
        }

        private float _fuelLevelPct;
        public float FuelLevelPct
        {
            get => _fuelLevelPct;
            internal set => SetProperty(ref _fuelLevelPct, value);
        }

        private long _pitSvFlags;
        public long PitSvFlags
        {
            get => _pitSvFlags;
            internal set => SetProperty(ref _pitSvFlags, value);
        }

        private float _pitSvLFP;
        public float PitSvLFP
        {
            get => _pitSvLFP;
            internal set => SetProperty(ref _pitSvLFP, value);
        }

        private float _pitSvRFP;
        public float PitSvRFP
        {
            get => _pitSvRFP;
            internal set => SetProperty(ref _pitSvRFP, value);
        }

        private float _pitSvLRP;
        public float PitSvLRP
        {
            get => _pitSvLRP;
            internal set => SetProperty(ref _pitSvLRP, value);
        }

        private float _pitSvRRP;
        public float PitSvRRP
        {
            get => _pitSvRRP;
            internal set => SetProperty(ref _pitSvRRP, value);
        }

        private float _pitSvFuel;
        public float PitSvFuel
        {
            get => _pitSvFuel;
            internal set => SetProperty(ref _pitSvFuel, value);
        }

        private float _replayPlaySpeed;
        public float ReplayPlaySpeed
        {
            get => _replayPlaySpeed;
            internal set => SetProperty(ref _replayPlaySpeed, value);
        }

        private bool _replayPlaySlowMotion;
        public bool ReplayPlaySlowMotion
        {
            get => _replayPlaySlowMotion;
            internal set => SetProperty(ref _replayPlaySlowMotion, value);
        }

        private double _replaySessionTime;
        public double ReplaySessionTime
        {
            get => _replaySessionTime;
            internal set => SetProperty(ref _replaySessionTime, value);
        }

        private int _replaySessionNum;
        public int ReplaySessionNum
        {
            get => _replaySessionNum;
            internal set => SetProperty(ref _replaySessionNum, value);
        }

        private bool _dcTractionControlToggle;
        public bool dcTractionControlToggle
        {
            get => _dcTractionControlToggle;
            internal set => SetProperty(ref _dcTractionControlToggle, value);
        }

        private float _dcBrakeBias;
        public float dcBrakeBias
        {
            get => _dcBrakeBias;
            internal set => SetProperty(ref _dcBrakeBias, value);
        }

        private float _dcTractionControl;
        public float dcTractionControl
        {
            get => _dcTractionControl;
            internal set => SetProperty(ref _dcTractionControl, value);
        }

        private float _dcABS;
        public float dcABS
        {
            get => _dcABS;
            internal set => SetProperty(ref _dcABS, value);
        }

        private float _dcThrottleShape;
        public float dcThrottleShape
        {
            get => _dcThrottleShape;
            internal set => SetProperty(ref _dcThrottleShape, value);
        }

        private float _dcFuelMixture;
        public float dcFuelMixture
        {
            get => _dcFuelMixture;
            internal set => SetProperty(ref _dcFuelMixture, value);
        }

        private float _waterTemp;
        public float WaterTemp
        {
            get => _waterTemp;
            internal set => SetProperty(ref _waterTemp, value);
        }

        private float _waterLevel;
        public float WaterLevel
        {
            get => _waterLevel;
            internal set => SetProperty(ref _waterLevel, value);
        }

        private float _fuelPress;
        public float FuelPress
        {
            get => _fuelPress;
            internal set => SetProperty(ref _fuelPress, value);
        }

        private float _fuelUsePerHour;
        public float FuelUsePerHour
        {
            get => _fuelUsePerHour;
            internal set => SetProperty(ref _fuelUsePerHour, value);
        }

        private float _oilTemp;
        public float OilTemp
        {
            get => _oilTemp;
            internal set => SetProperty(ref _oilTemp, value);
        }

        private float _oilPress;
        public float OilPress
        {
            get => _oilPress;
            internal set => SetProperty(ref _oilPress, value);
        }

        private float _oilLevel;
        public float OilLevel
        {
            get => _oilLevel;
            internal set => SetProperty(ref _oilLevel, value);
        }

        private float _voltage;
        public float Voltage
        {
            get => _voltage;
            internal set => SetProperty(ref _voltage, value);
        }

        private float _manifoldPress;
        public float ManifoldPress
        {
            get => _manifoldPress;
            internal set => SetProperty(ref _manifoldPress, value);
        }

        private float _RRbrakeLinePress;
        public float RRbrakeLinePress
        {
            get => _RRbrakeLinePress;
            internal set => SetProperty(ref _RRbrakeLinePress, value);
        }

        private float _RRcoldPressure;
        public float RRcoldPressure
        {
            get => _RRcoldPressure;
            internal set => SetProperty(ref _RRcoldPressure, value);
        }

        private float _RRtempCL;
        public float RRtempCL
        {
            get => _RRtempCL;
            internal set => SetProperty(ref _RRtempCL, value);
        }

        private float _RRtempCM;
        public float RRtempCM
        {
            get => _RRtempCM;
            internal set => SetProperty(ref _RRtempCM, value);
        }

        private float _RRtempCR;
        public float RRtempCR
        {
            get => _RRtempCR;
            internal set => SetProperty(ref _RRtempCR, value);
        }

        private float _RRwarL;
        public float RRwearL
        {
            get => _RRwarL;
            internal set => SetProperty(ref _RRwarL, value);
        }

        private float _RRwearM;
        public float RRwearM
        {
            get => _RRwearM;
            internal set => SetProperty(ref _RRwearM, value);
        }

        private float _RRwearR;
        public float RRwearR
        {
            get => _RRwearR;
            internal set => SetProperty(ref _RRwearR, value);
        }

        private float _LRbrakeLinePress;
        public float LRbrakeLinePress
        {
            get => _LRbrakeLinePress;
            internal set => SetProperty(ref _LRbrakeLinePress, value);
        }

        private float _LRcoldPressure;
        public float LRcoldPressure
        {
            get => _LRcoldPressure;
            internal set => SetProperty(ref _LRcoldPressure, value);
        }

        private float _LRtempCL;
        public float LRtempCL
        {
            get => _LRtempCL;
            internal set => SetProperty(ref _LRtempCL, value);
        }

        private float _LRtempCM;
        public float LRtempCM
        {
            get => _LRtempCM;
            internal set => SetProperty(ref _LRtempCM, value);
        }

        private float _LRtempCR;
        public float LRtempCR
        {
            get => _LRtempCR;
            internal set => SetProperty(ref _LRtempCR, value);
        }

        private float LR_wearL;
        public float LRwearL
        {
            get => LR_wearL;
            internal set => SetProperty(ref LR_wearL, value);
        }

        private float _LRwearM;
        public float LRwearM
        {
            get => _LRwearM;
            internal set => SetProperty(ref _LRwearM, value);
        }

        private float _LRwearR;
        public float LRwearR
        {
            get => _LRwearR;
            internal set => SetProperty(ref _LRwearR, value);
        }

        private float _RFbrakeLinePressure;
        public float RFbrakeLinePressure
        {
            get => _RFbrakeLinePressure;
            internal set => SetProperty(ref _RFbrakeLinePressure, value);
        }

        private float _RFcoldPressure;
        public float RFcoldPressure
        {
            get => _RFcoldPressure;
            internal set => SetProperty(ref _RFcoldPressure, value);
        }

        private float _RFtempCL;
        public float RFtempCL
        {
            get => _RFtempCL;
            internal set => SetProperty(ref _RFtempCL, value);
        }

        private float _RFtempCM;
        public float RFtempCM
        {
            get => _RFtempCM;
            internal set => SetProperty(ref _RFtempCM, value);
        }

        private float _RFtempCR;
        public float RFtempCR
        {
            get => _RFtempCR;
            internal set => SetProperty(ref _RFtempCR, value);
        }

        private float _RFwearL;
        public float RFwearL
        {
            get => _RFwearL;
            internal set => SetProperty(ref _RFwearL, value);
        }

        private float _RFwearM;
        public float RFwearM
        {
            get => _RFwearM;
            internal set => SetProperty(ref _RFwearM, value);
        }

        private float _RFwearR;
        public float RFwearR
        {
            get => _RFwearR;
            internal set => SetProperty(ref _RFwearR, value);
        }

        private float _LFbrakeLinePress;
        public float LFbrakeLinePress
        {
            get => _LFbrakeLinePress;
            internal set => SetProperty(ref _LFbrakeLinePress, value);
        }

        private float _LFcoldPressure;
        public float LFcoldPressure
        {
            get => _LFcoldPressure;
            internal set => SetProperty(ref _LFcoldPressure, value);
        }

        private float _LFtempCL;
        public float LFtempCL
        {
            get => _LFtempCL;
            internal set => SetProperty(ref _LFtempCL, value);
        }

        private float _LFtempCM;
        public float LFtempCM
        {
            get => _LFtempCM;
            internal set => SetProperty(ref _LFtempCM, value);
        }

        private float _LFtempCR;
        public float LFtempCR
        {
            get => _LFtempCR;
            internal set => SetProperty(ref _LFtempCR, value);
        }

        private float _LFwearL;
        public float LFwearL
        {
            get => _LFwearL;
            internal set => SetProperty(ref _LFwearL, value);
        }

        private float _LFwearM;
        public float LFwearM
        {
            get => _LFwearM;
            internal set => SetProperty(ref _LFwearM, value);
        }

        private float _LFwearR;
        public float LFwearR
        {
            get => _LFwearR;
            internal set => SetProperty(ref _LFwearR, value);
        }

        private float _RRshockDefl;
        public float RRshockDefl
        {
            get => _RRshockDefl;
            internal set => SetProperty(ref _RRshockDefl, value);
        }

        private float _RRshockVel;
        public float RRshockVel
        {
            get => _RRshockVel;
            internal set => SetProperty(ref _RRshockVel, value);
        }

        private float _LRshockDefl;
        public float LRshockDefl
        {
            get => _LRshockDefl;
            internal set => SetProperty(ref _LRshockDefl, value);
        }

        private float _LRshockVel;
        public float LRshockVel
        {
            get => _LRshockVel;
            internal set => SetProperty(ref _LRshockVel, value);
        }

        private float _RFshockDefl;
        public float RFshockDefl
        {
            get => _RFshockDefl;
            internal set => SetProperty(ref _RFshockDefl, value);
        }

        private float _RFshockVel;
        public float RFshockVel
        {
            get => _RFshockVel;
            internal set => SetProperty(ref _RFshockVel, value);
        }

        private float _LFshockDefl;
        public float LFshockDefl
        {
            get => _LFshockDefl;
            internal set => SetProperty(ref _LFshockDefl, value);
        }

        private float _LFshockVel;
        public float LFshockVel
        {
            get => _LFshockVel;
            internal set => SetProperty(ref _LFshockVel, value);
        }

        private readonly PropertyInfo[] _properties = typeof(Telemetry).GetProperties(BindingFlags.Instance | BindingFlags.Public);

        internal void UpdateTelemetryData(iRacingSDK sdk)
        {
            foreach (var property in _properties)
            {
                var value = sdk.GetData(property.Name);
                // ReSharper disable once CanBeReplacedWithTryCastAndCheckForNull
                if (value is float[] && property.PropertyType == typeof(float))
                {
                    property.SetValue(this, ((float[])value).LastOrDefault());
                }
                else
                {
                    property.SetValue(this, value);
                }
            }
        }
    }
}
