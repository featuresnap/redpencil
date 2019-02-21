namespace Tests

open System
open Xunit

module RedPencilTest = 

    open Xunit
    open RedPencil.Library

    [<Fact>]
    let ``Start State, time passes, counts down days till eligible`` () =
        let currentState = Ineligible {DaysUntilEligible = 30; LastPrice = 100M }
        let transition = DaysElapse(29)
        let expectedState = Ineligible {DaysUntilEligible = 1; LastPrice = 100M }
        Assert.Equal(expectedState, currentState |> applyTransition(transition))
       
    [<Fact>]
    let ``Start State, price decrease, restarts countdown with new price`` () =
        let currentState = Ineligible {DaysUntilEligible = 1; LastPrice = 100M }
        let transition = PriceDecrease(10M)
        let expectedState = Ineligible {DaysUntilEligible = 30; LastPrice = 90M }
        Assert.Equal(expectedState, currentState |> applyTransition(transition))
          
    [<Fact>]
    let ``Start State, price increase, restarts countdown with new price`` () =
        let currentState = Ineligible {DaysUntilEligible = 1; LastPrice = 100M }
        let transition = PriceIncrease(10M)
        let expectedState = Ineligible {DaysUntilEligible = 30; LastPrice = 110M }
        Assert.Equal(expectedState, currentState |> applyTransition(transition))
          
    [<Fact>]
    let ``Start State, 30 days pass, transition to Eligible`` () =
        let currentState = Ineligible {DaysUntilEligible = 1; LastPrice = 100M }
        let transition = DaysElapse(30)
        let expectedState = Eligible {Lower = 70M; Upper = 95M }
        Assert.Equal(expectedState, currentState |> applyTransition(transition))
       