var sheetId = SpreadsheetApp.openById("10fMEvmLLreOmNsWWwMVQY7bHbgKYTxjBPAEUP73AbCw");
var app = SpreadsheetApp;
var sheet = app.getActiveSpreadsheet();
var activeSheet = app.getActiveSpreadsheet().getActiveSheet();
let date = new Date();
var TEMPLATE_SHEET = "템플릿";
var p;

var year = date.getFullYear();
var month = ('0' + (date.getMonth() + 1)).slice(-2);
var day = ('0' + date.getDate()).slice(-2);
var dateString = year + '-' + month  + '-' + day;

const createSheetForToday = () => {
  const today = dateString;

  sheetId.setActiveSheet(sheetId.getSheetByName(TEMPLATE_SHEET), true);
  sheetId.duplicateActiveSheet();
  
  sheetId.setActiveSheet(sheetId.getSheetByName(`${TEMPLATE_SHEET}의 사본`), true);
  try {
    sheetId.getActiveSheet().setName(today);
  } catch (e) {
    sheetId.deleteActiveSheet();
  }
 
  
};

function doPost(e)
{
  createSheetForToday();
  var targetSheet = app.getActiveSpreadsheet().getSheetByName(dateString);
  p = e.parameter;
  var lastrow = targetSheet.getLastRow() + 1;
  targetSheet.getRange(lastrow , 1).setValue(p.value);
  targetSheet.getRange(lastrow , 2).setValue(date.toLocaleString());
  return ContentService.createTextOutput(p.value);
}