module LogMoudle
open System.IO
open System.Data
open System.Text.RegularExpressions

type ActionMode = Send|Recv|Null
type MsgItem = {Time : string;CommandID :string;JsonMsg:string;ServerID:string; Actionmode:ActionMode;}
   
let mtime = @"\d{4}-\d{2}-\d{2}\s\d{2}:\d{2}:\d{2}"
let mSendAction = @"SendCmd"
let mrecvAction = @"Call\sBack"
let mserverID = @"\d{3}\.\d{3}\.\d{1,}\.\d{1,}\s\d{4,}"
let mCommandID = @"CommandID\,\d{4,}"
let mcmdType = @"cmdType\sis\s\:\s{1,}\d{4,}"
let mjson = @"{[\S\s]+}"

let mathcSometing str matc =
    let re = Regex.Match(str,matc)
    if re.Success then re.Value else "null"

let getItem str =
    let commandID = mathcSometing str mCommandID
                   |> (fun str->str.Split(','))
                   |> (fun strs->if strs.Length <> 1 then strs.[1] else strs.[0])

    let cmdType =  mathcSometing str mcmdType
                   |>(fun str->str.Split(':'))
                   |>(fun strs->if strs.Length <> 1 then strs.[1] else strs.[0])

    let CommandId = match commandID,cmdType with
                    | c,"null" -> c
                    |"null",d -> d
                    |_ ->"null"

    let act=match (mathcSometing str mSendAction,mathcSometing str mrecvAction) with
            |"null","null"->ActionMode.Null
            |_,"null" -> ActionMode.Send
            |"null",_ -> ActionMode.Recv
            |_ ->ActionMode.Null
    {
       Time = mathcSometing str mtime;
       CommandID = CommandId;
       JsonMsg = mathcSometing str mjson; 
       ServerID = mathcSometing str mserverID;
       Actionmode = act;
    }

let getMsgItems path=
    File.ReadAllText(path)
    |>Regex("\[Info\]:").Split
    |>Seq.map (fun info->getItem info)

let getTbale items  =
    let table=new DataTable()
    ["Time";"CommandID";"Actionmode";"JsonMsg";"ServerID";]|>Seq.iter (fun name-> table.Columns.Add(name)|>ignore)
    items|>Seq.iter (fun row->table.Rows.Add(row.Time,row.CommandID,row.Actionmode,row.JsonMsg,row.ServerID)|>ignore)
    table

let GetTableByPath path = 
    path|> getMsgItems |> getTbale

