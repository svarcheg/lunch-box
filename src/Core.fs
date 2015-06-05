namespace btcore

open System
open LanguagePrimitives

module Engine = 
    type UserId = 
        | UserId of string
    
    type InstrumentCode = 
        | InstrumentCode of string
    
    type Side = 
        | Buy
        | Sell
    
    type Transaction<[<Measure>] 'currency> = 
        { Date : DateTime
          Side : Side
          InstrumentCode : InstrumentCode
          Price : decimal<'currency>
          Quantity : int }
    
    type Account<[<Measure>] 'currency> = 
        { Balance : decimal<'currency>
          Portfolio : Map<InstrumentCode, int>
          Transactions : List<Transaction<'currency>> }
    
    type OrderType = 
        | Market
    
    type MarketData<[<Measure>] 'currency> = 
        | MarketData of Map<InstrumentCode, decimal<'currency>>
    
    type Order<[<Measure>] 'currency> = 
        { OrderType : OrderType
          Side : Side
          InstrumentCode : InstrumentCode
          Quantity : int
          Account : Account<'currency> }

    type Result<'TSuccess,'TFailure> = 
    | Success of 'TSuccess
    | Failure of 'TFailure
    
    let createAccount (balance : decimal<'currency>) = 
        { Balance = balance
          Portfolio = Map []
          Transactions = [] }
    
    let absoluteCapi (transaction : Transaction<'currency>) = (decimal transaction.Quantity) * transaction.Price
    
    let updateBalanceWith (transaction : Transaction<'currency>) account = 
        let operation = 
            match transaction.Side with
            | Buy -> (-)
            | Sell -> (+)
        { account with Balance = (operation account.Balance (absoluteCapi transaction)) }
    
    let updatePortfolioWith (transaction : Transaction<'currency>) account = 
        let quantity = 
            match account.Portfolio.TryFind transaction.InstrumentCode with
            | Some v -> v
            | None -> 0
        
        let operation = 
            match transaction.Side with
            | Buy -> (+)
            | Sell -> (-)
        
        let nextQuantity = operation quantity transaction.Quantity
        { account with Portfolio = account.Portfolio.Add(transaction.InstrumentCode, nextQuantity) }
    
    let updateTransactionsWith transaction account = { account with Transactions = transaction :: account.Transactions }
    
    let processTransaction account transaction = 
        account
        |> updateBalanceWith transaction
        |> updatePortfolioWith transaction
        |> updateTransactionsWith transaction
    
    let isIdentified (UserId username) = true

    let identifyUser username : Result<UserId, string> = Success (UserId username)

    let getPrice (MarketData marketData) instrumentCode = marketData.TryFind instrumentCode
    
    let executeMarketOrder marketData (order : Order<'currency>) now : option<Transaction<'currency>> = 
        let price = getPrice marketData order.InstrumentCode
        match price with
        | Some px -> 
            Some { Date = now
                   Side = order.Side
                   InstrumentCode = order.InstrumentCode
                   Price = px
                   Quantity = order.Quantity }
        | None -> None
    
    let executeOrder marketData order = 
        match order.OrderType with
        | Market -> executeMarketOrder marketData order

