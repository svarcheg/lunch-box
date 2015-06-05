namespace test.Core

open System
open btcore.Engine
open NUnit.Framework
open FsUnit
open btcore.Currency

module Engine = 
    [<Test>]
    let ``create empty account``() = 
        createAccount 10.0m<RUB> |> should equal { Balance = 10.0m<RUB>
                                                   Portfolio = Map.empty
                                                   Transactions = List.empty }
    
    [<Test>]
    let ``compute capi of a transaction``() = 
        absoluteCapi { Date = DateTime.Now
                       Side = Buy
                       InstrumentCode = InstrumentCode "FTE.PA"
                       Price = 10.0m<RUB>
                       Quantity = 10 }
        |> should equal 100.0m<RUB>
    
    [<Test>]
    let ``update account balance with buy transaction``() = 
        updateBalanceWith { Date = DateTime.Now
                            Side = Buy
                            InstrumentCode = InstrumentCode "FTE.PA"
                            Price = 1.0m<RUB>
                            Quantity = 5 } (createAccount 10.0m<RUB>)
        |> should equal { Balance = 5.0M
                          Portfolio = Map []
                          Transactions = [] }
    
    [<Test>]
    let ``update account balance with sell transaction``() = 
        updateBalanceWith { Date = DateTime.Now
                            Side = Sell
                            InstrumentCode = InstrumentCode "FTE.PA"
                            Price = 1.0m<RUB>
                            Quantity = 5 } (createAccount 10.0m<RUB>)
        |> should equal { Balance = 15.0M
                          Portfolio = Map []
                          Transactions = [] }
    
    [<Test>]
    let ``update position with buy transaction should update portfolio``() = 
        updatePortfolioWith { Date = DateTime.Now
                              Side = Buy
                              InstrumentCode = InstrumentCode "FTE.PA"
                              Price = 1.0m<RUB>
                              Quantity = 10 } { Balance = 15.0m<RUB>
                                                Portfolio = Map []
                                                Transactions = [] }
        |> should equal { Balance = 15.0m<RUB>
                          Portfolio = Map [ (InstrumentCode "FTE.PA", 10) ]
                          Transactions = [] }
    
    [<Test>]
    let ``update position with sell transaction should update portfolio``() = 
        updatePortfolioWith { Date = DateTime.Now
                              Side = Sell
                              InstrumentCode = InstrumentCode "FTE.PA"
                              Price = 1.0m<RUB>
                              Quantity = 10 } { Balance = 15.0m<RUB>
                                                Portfolio = Map []
                                                Transactions = [] }
        |> should equal { Balance = 15.0m<RUB>
                          Portfolio = Map [ (InstrumentCode "FTE.PA", -10) ]
                          Transactions = [] }
    
    [<Test>]
    let ``update transactions should add the transaction to transaction list``() = 
        let transaction = 
            { Date = DateTime.Now
              Side = Buy
              InstrumentCode = InstrumentCode "FTE.PA"
              Price = 10.0m<RUB>
              Quantity = 10 }
        
        let account = createAccount 10.0m<RUB>
        updateTransactionsWith transaction account |> should equal { account with Transactions = [ transaction ] }
    
    [<Test>]
    let ``process transaction should return fully updated account``() = 
        let transaction = 
            { Date = DateTime.Now
              Side = Buy
              InstrumentCode = InstrumentCode "FTE.PA"
              Price = 1.0m<RUB>
              Quantity = 10 }
        
        let account = createAccount 10.0m<RUB>
        processTransaction account transaction |> should equal { Balance = 0.0m<RUB>
                                                                 Portfolio = Map [ (InstrumentCode "FTE.PA", 10) ]
                                                                 Transactions = [ transaction ] }