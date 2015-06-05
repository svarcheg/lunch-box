module StepDefinitions

open TickSpec
open NUnit.Framework

open btcore.Engine
open btcore.Currency

type State = {User : option<UserId>;Account : option<Account<RUB>>}

let mutable state = {User = None;Account = None}

let [<Given>] ``^(.+) is identified$`` (userName : string) = 
    state <- match identifyUser userName with
            | Success id -> {state with User = Some id}
            | Failure err -> {state with User = None}

let [<Given>] ``^s?he has ([0-9]+) on (?:his|her) account$``(amount : int) = 
    state <- {state with Account = Some (createAccount ((decimal amount) *  1.0m<RUB>))}

let [<Given>] ``^(\w+) is selling at ([0-9]+) per share$``(instrument: string) (price : int) = ()
let [<When>] ``^s?he buys ([0-9]+) shares of (\w+)$``(quantity : int) (instrument: string) = ()
let [<Then>] ``^s?he should get a confirmation of success$``() = ()
let [<Then>] ``^h(?:is|er) account balance should be ([0-9]+)$``(amount :int) = ()
let [<Then>] ``^h(?:is|er) portfolio should contain ([0-9]+) shares? of (\w+)$`` (quantity :int) (symbol : string) = ()
let [<Given>] ``^h(?:is|er) portfolio contains ([0-9]+) shares of (\w+)$`` (quantity :int) (symbol: string) = ()
let [<When>] ``^s?he sells ([0-9]+) shares of (\w+)$`` (quantity :int) (symbol: string)  = ()
let [<Given>] ``^(.+)'s portfolio contains ([0-9]+) shares of (\w+)$`` (user :string) (quantity : int) (symbol: string) = ()
let [<Then>] ``^s?he should be rejected$``() = ()
