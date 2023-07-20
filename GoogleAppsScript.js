var sheetId = SpreadsheetApp.openById("10fMEvmLLreOmNsWWwMVQY7bHbgKYTxjBPAEUP73AbCw");
var app = SpreadsheetApp;
var sheet = app.getActiveSpreadsheet();
var activeSheet = app.getActiveSpreadsheet().getActiveSheet();
let date = new Date();
var template = "템플릿";
var p;


var year = date.getFullYear();
var month = ('0' + (date.getMonth() + 1)).slice(-2);
var day = ('0' + date.getDate()).slice(-2);
var dateString = year + '-' + month  + '-' + day;

var sheetid = sheet.getSheetByName(dateString).getSheetId();

var timeString = date.getHours();

const createSheet = () => {
  const today = dateString;

  sheetId.setActiveSheet(sheetId.getSheetByName(template), true);
  sheetId.duplicateActiveSheet();
  
  sheetId.setActiveSheet(sheetId.getSheetByName(`${template}의 사본`), true);
  try {
    sheetId.getActiveSheet().setName(today);
  } catch (e) {
    sheetId.deleteActiveSheet();
  }
 
  
};


function doPost(e)
{
  createSheet();
  var targetSheet = sheet.getSheetByName(dateString);
  p = e.parameter;
  var lastrow = targetSheet.getLastRow() + 1;
  if (timeString >= 18)
  {
    targetSheet.getRange(lastrow , 1).setValue(p.value).setBackground('#ea4336');
    targetSheet.getRange(lastrow , 2).setValue(date.toLocaleString()).setBackground('#ea4336');
  } else {
    targetSheet.getRange(lastrow , 1).setValue(p.value).setBackground('#34a853');
    targetSheet.getRange(lastrow , 2).setValue(date.toLocaleString()).setBackground('#34a853');
  }
  return ContentService.createTextOutput(p.value);
}

function SendEmail(){
	try {
 
    var emailSheet = sheet.getSheetByName("DB"); 

    for(let index=2; index<99; index++){ 

      email = emailSheet.getRange("A"+index).getValue();

      if (email !==  ""){ 

        MailApp.sendEmail({
          to : email, 
          subject : "test email",
          body : "https://docs.google.com/spreadsheets/d/10fMEvmLLreOmNsWWwMVQY7bHbgKYTxjBPAEUP73AbCw/export?format=xlsx&gid=" + sheetid,
        });

      }else{
        break; 
      }
                    
    }
        
     } catch(err){
     	console.log(err);     
     }
}

function setTrigger(){ 

  const time = new Date(); 
  time.setHours(20); 
  time.setMinutes(20); 
  ScriptApp.newTrigger('SendEmail').timeBased().at(time).create(); 

}
