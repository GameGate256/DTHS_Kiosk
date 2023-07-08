var sheetId = SpreadsheetApp.openById("1_Ywz7GgZvH624iyzNC8t1l0ujsXpIn16Vjfp40w-__A");
var sheet = sheetId.getSheets()[0];
var p;
let toady = new Date();


function doPost(e)
{
  p = e.parameter;
  var lastrow = sheet.getLastRow() + 1;
  sheet.getRange(lastrow , 1).setValue(p.value);
  sheet.getRange(lastrow , 2).setValue(toady.toLocaleString());
  return ContentService.createTextOutput(p.value);
}