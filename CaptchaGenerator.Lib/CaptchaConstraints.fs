namespace CaptchaGenerator.Lib

open System

type internal CaptchaConstraints(?minFontSize0 : int, ?maxFontSize0 : int, ?minAngle0 : int, ?maxAngle0 : int, ?numberOfRandomLines0 : int, ?randomLineWidth0 : int) = 
    // defaults
    member this.minFontSize = defaultArg minFontSize0 25
    member this.maxFontSize = defaultArg maxFontSize0 50
    member this.minAngle = defaultArg minAngle0 -40
    member this.maxAngle = defaultArg maxAngle0 40
    member this.numberOfRandomLines = defaultArg numberOfRandomLines0 4
    member this.randomLineWidth = defaultArg randomLineWidth0 2
    
    member this.TestCaptchaConstraints() = 
        if this.minFontSize < 0 then 
            raise <| new CaptchaGeneratorException("Min font size " + this.minFontSize.ToString() + " is less than 0")
        if this.maxFontSize < 0 then 
            raise <| new CaptchaGeneratorException("Max font size " + this.minFontSize.ToString() + " is less than 0")
        if this.minFontSize > this.maxFontSize then 
            raise 
            <| new CaptchaGeneratorException("Min font size" + this.minFontSize.ToString() + " is greater than " 
                                             + this.maxFontSize.ToString())
        if this.minAngle < -360 || this.minAngle > 360 then 
            raise 
            <| new CaptchaGeneratorException("Min angle size " + this.minAngle.ToString() 
                                             + " is less than -360 or greater than 360")
        if this.maxAngle < -360 || this.maxAngle > 360 then 
            raise 
            <| new CaptchaGeneratorException("Max angle size " + this.maxAngle.ToString() 
                                             + " is less than -360 or greater than 360")
        if this.minAngle > this.maxAngle then 
            raise 
            <| new CaptchaGeneratorException("Min angle size " + this.minAngle.ToString() + " is greater than " 
                                             + this.maxAngle.ToString())
        if this.numberOfRandomLines < 0 then 
            raise 
            <| new CaptchaGeneratorException("Number of random lines " + this.numberOfRandomLines.ToString() 
                                             + " is less than 0")
        if this.randomLineWidth < 1 then 
            raise 
            <| new CaptchaGeneratorException("Random line width " + this.randomLineWidth.ToString() 
                                             + " is less than 1px")
    
    member this.RandomFontSizeFromConstraints() = 
        let random = new Random(Guid.NewGuid().GetHashCode())
        random.Next(this.minFontSize, this.maxFontSize)
    
    member this.RandomAngleFromConstraints() = 
        let random = new Random(Guid.NewGuid().GetHashCode())
        random.Next(this.minAngle, this.maxAngle)
