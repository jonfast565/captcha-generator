namespace CaptchaGenerator.Lib

open System

type CaptchaGeneratorException = 
    inherit Exception
    new(message : string) = { inherit Exception(message) }
    new(message : string, innerException : Exception) = { inherit Exception(message, innerException) }
