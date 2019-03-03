namespace Tests

open System
open Xunit

module WhenIneligible = 

    open Xunit
    open RedPencil.Library
    

    [<Fact>]
    let ``Passage of time is counted toward days until eligible`` () =
        let currentState = Ineligible {DaysUntilEligible = 30; LastPrice = 100M }
        let transition = DaysElapse(29)
        let expectedState = Ineligible {DaysUntilEligible = 1; LastPrice = 100M }
        Assert.Equal(expectedState, currentState |> applyTransition(transition))
       
    [<Fact>]
    let ``Price decrease restarts eligibility countdown with new price`` () =
        let currentState = Ineligible {DaysUntilEligible = 1; LastPrice = 100M }
        let transition = PriceDecrease(10M)
        let expectedState = Ineligible {DaysUntilEligible = 30; LastPrice = 90M }
        Assert.Equal(expectedState, currentState |> applyTransition(transition))
          
    [<Fact>]
    let ``Price increase restarts eligibility countdown with new price`` () =
        let currentState = Ineligible {DaysUntilEligible = 1; LastPrice = 100M }
        let transition = PriceIncrease(10M)
        let expectedState = Ineligible {DaysUntilEligible = 30; LastPrice = 110M }
        Assert.Equal(expectedState, currentState |> applyTransition(transition))
          
    [<Fact>]
    let ``30 days with stable price results in eligibility`` () =
        let currentState = Ineligible {DaysUntilEligible = 1; LastPrice = 100M }
        let transition = DaysElapse(30)
        let expectedState = Eligible {Lower = 70M; Upper = 95M }
        Assert.Equal(expectedState, currentState |> applyTransition(transition))
       