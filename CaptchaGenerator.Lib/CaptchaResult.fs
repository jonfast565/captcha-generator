namespace CaptchaGenerator.Lib

open System

type CaptchaResult(captchaValue : string, captchaImage : byte []) = 
    member val CaptchaValue : string = captchaValue with get, set
    member val CaptchaImage : byte [] = captchaImage with get, set
