var sheetId = SpreadsheetApp.openById("1_Ywz7GgZvH624iyzNC8t1l0ujsXpIn16Vjfp40w-__A");
var sheet = sheetId.getSheets()[0];
var p;


function doPost(e)
{
  p = e.parameter;
  sheet.getRange(1,2).setValue(p.value);
  return ContentService.createTextOutput(p.value);
}