namespace CaptchaGenerator.Lib

open System

type internal CaptchaStringGenerator(length : int) = 
    let alphanumericChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"
    
    let SelectRandomAlphanumericStringValue() : string = 
        let random = new Random(Guid.NewGuid().GetHashCode())
        let next = random.Next(0, alphanumericChars.Length)
        alphanumericChars.[next].ToString()
    
    let rec GenerateDecreasingCaptchaStringInternal(counter : int, resultString : string) = 
        let counterDecremented = counter - 1
        let newResultString = resultString + SelectRandomAlphanumericStringValue()
        match counter with
        | counter when counter > 0 -> GenerateDecreasingCaptchaStringInternal(counterDecremented, newResultString)
        | _ -> resultString
    
    member val Length : int = length with get, set
    member this.GenerateString() : string = GenerateDecreasingCaptchaStringInternal(this.Length, String.Empty)
