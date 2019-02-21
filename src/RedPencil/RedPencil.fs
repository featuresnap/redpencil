namespace RedPencil

module Library = 

    type Price = decimal
    type Amount = decimal

    let price: Price = 0M

    type IneligibleData = { DaysUntilEligible: int; LastPrice: Price}
    type PriceBounds = {Lower: Price; Upper: Price}
    type PromotionData = { MinimumPrice: Price; DaysUntilExpiration: int}

    type PromotionState = 
        | Ineligible of IneligibleData
        | Eligible of PriceBounds
        | Active of PromotionData
        
    type Transition = 
        | DaysElapse of int
        | PriceIncrease of Amount
        | PriceDecrease of Amount

    let handleIneligibleTransitions data transition =
        match transition with 
            |DaysElapse days -> 
                let remainingDays = data.DaysUntilEligible - days
                let upper = data.LastPrice * 0.95M
                let lower = data.LastPrice * 0.70M
                if remainingDays <= 0 then Eligible {Lower = lower; Upper = upper}
                else Ineligible {data with DaysUntilEligible = remainingDays}
            |PriceDecrease(amount) -> 
                let newPrice = data.LastPrice - amount
                Ineligible {DaysUntilEligible = 30; LastPrice = newPrice}           
            |PriceIncrease(amount) -> 
                let newPrice = data.LastPrice + amount
                Ineligible {DaysUntilEligible = 30; LastPrice = newPrice}           
        
    let applyTransition transition currentState = 
        match currentState with 
        |Ineligible data -> handleIneligibleTransitions data transition
        |_ -> failwith "handle other states"
