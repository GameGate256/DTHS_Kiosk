//스프레드 시트의 id로 시트를 불러옴
var sheetId = SpreadsheetApp.openById("10fMEvmLLreOmNsWWwMVQY7bHbgKYTxjBPAEUP73AbCw");
var app = SpreadsheetApp;
var sheet = app.getActiveSpreadsheet();
var activeSheet = app.getActiveSpreadsheet().getActiveSheet();
//날짜를 요일, 월, 일 시간, 표준시로 나타냄
let date = new Date();
var template = "템플릿";
var p;

//년도를 가져옴
var year = date.getFullYear();
//날짜를 월만 저장되게 자름
var month = ('0' + (date.getMonth() + 1)).slice(-2);
//날짜를 일만 저장되게 자름
var day = ('0' + date.getDate()).slice(-2);
//자른 시간을 년도, 월, 일 로 나타냄
var dateString = year + '-' + month  + '-' + day;



var timeString = date.getHours();
//날짜에 맞춰 새 시트를 생성하는 함수
const createSheet = () => {
  const today = dateString;
  //템플릿 이라는 이름의 시트를 복사함
  sheetId.setActiveSheet(sheetId.getSheetByName(template), true);
  sheetId.duplicateActiveSheet();
  sheetId.setActiveSheet(sheetId.getSheetByName(`${template}의 사본`), true);
  try 
  {
    //복사한 시트의 이름을 현재 날짜로 변경
    sheetId.getActiveSheet().setName(today);
  } catch (e) 
  {
    //이미 존재한다면 복제한 시트를 삭제함
    sheetId.deleteActiveSheet();
  }
 
  
};

//유니티에서 값을 가져오는 함수
function doPost(e)
{
  //시트생성 함수를 실행
  createSheet();
  var targetSheet = sheet.getSheetByName(dateString);
  p = e.parameter;
  //시트 정렬을 위해 마지막 행의 밑으로 지정
  var lastrow = targetSheet.getLastRow() + 1;
  //18시 기준으로
  if (timeString >= 18)
  {
    //18시 이후면 빨간색으로 셀을 칠함
    targetSheet.getRange(lastrow , 1).setValue(p.value).setBackground('#ea4336');
    targetSheet.getRange(lastrow , 2).setValue(date.toLocaleString()).setBackground('#ea4336');
  } else {
    //18시 이전이면 초록색으로 셀을 칠함
    targetSheet.getRange(lastrow , 1).setValue(p.value).setBackground('#34a853');
    targetSheet.getRange(lastrow , 2).setValue(date.toLocaleString()).setBackground('#34a853');
  }
  //유니티로 값을 돌려보냄
  return ContentService.createTextOutput(p.value);
}

//이메일 전송 함수
function SendEmail()
{
	try 
  {
    var sheetnum = sheet.getSheetByName(dateString).getSheetId();
    //DB라는 이름의 시트를 가져옴
    var emailSheet = sheet.getSheetByName("DB"); 

    //DB 시트에 적힌 이메일을 가져옴
    for(let index=2; index<99; index++){ 

      email = emailSheet.getRange("A"+index).getValue();

      if (email !==  "")
      { 
        //이메일 전송: 
        MailApp.sendEmail({
          to : email, //이메일 주소
          subject : "test email",//이메일 제목
          //이메일 내용
          body : "https://docs.google.com/spreadsheets/d/10fMEvmLLreOmNsWWwMVQY7bHbgKYTxjBPAEUP73AbCw/export?format=xlsx&gid=" + sheetnum,
        });

      }else
      {
        break; 
      }
                    
    }
        
     } catch(err)
     {
     	console.log(err);     
     }
}

//이메일 전송 자동실행 트리거 함수(구글 시트에서 따로 설정필요)
function setTrigger()
{ 

  const time = new Date(); 
  time.setHours(20); 
  time.setMinutes(20); 
  ScriptApp.newTrigger('SendEmail').timeBased().at(time).create(); 

}
