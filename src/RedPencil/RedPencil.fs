namespace RedPencil

module Library = 

    type Price = decimal
    type Amount = decimal

    let price: Price = 0M

    type IneligibleData = { DaysUntilEligible: int; LastPrice: Price}
    type PriceBounds = {Lower: Price; Upper: Price}
    type PromotionData = { MinimumPrice: Price; DaysUntilExpiration: int}

    type PromotionState = 
        | IneligibleForPromotion of IneligibleData
        | EligibleForPromotion of PriceBounds
        | InPromotion of PromotionData
        
    type Transition = 
        | DaysElapse of int
        | PriceIncrease of Amount
        | PriceDecrease of Amount

    let handleIneligibleForPromotionTransitions data transition =
        match transition with 
            |DaysElapse days -> 
                let remainingDays = data.DaysUntilEligible - days
                let upper = data.LastPrice * 0.95M
                let lower = data.LastPrice * 0.70M
                if remainingDays <= 0 then EligibleForPromotion {Lower = lower; Upper = upper}
                else IneligibleForPromotion {data with DaysUntilEligible = remainingDays}
            |PriceDecrease(amount) -> 
                let newPrice = data.LastPrice - amount
                IneligibleForPromotion {DaysUntilEligible = 30; LastPrice = newPrice}           
            |PriceIncrease(amount) -> 
                let newPrice = data.LastPrice + amount
                IneligibleForPromotion {DaysUntilEligible = 30; LastPrice = newPrice}           
        
    let applyTransition transition currentState = 
        match currentState with 
        |IneligibleForPromotion data -> handleIneligibleForPromotionTransitions data transition
        |_ -> failwith "handle other states"
