using System;
using System.Collections.Generic;
using System.Text;
namespace Lib.Utils
{
	public class SystemConstants
	{
		public static string Invalid_Content = "Nội dung vi phạm luật phòng chống tấn công mạng";
		public static string Invalid_Email = "Thư điện tử không hợp lệ";
		public static string Invalid_FullName = "Họ và tên không hợp lệ";
		public static string Invalid_Isdn = "Số điện thoại không hợp lệ";
		public static string Invalid_UserName = "Tên tài khoản không hợp lệ";
		public static string Invalid_DateTime = "Sai định dạng ngày tháng";

		public const short BeginYear = 2016;
		public const string Infor_ConfirmDelete = "Quí vị thực sự muốn xóa?";
		public const string Infor_ConfirmDeleteJs = "return confirm('Quí vị thực sự muốn xóa?')";
		public static string Infor_Granted = "Đã gán";
		public static string Infor_InsertSucc = "Đã thêm bản ghi";
		public static string Infor_UpdateSucc = "Đã cập nhật bản ghi";
		public static string Infor_Delete = "Xóa";
		public static string Infor_Reset = "Làm lại";
		public static string Infor_DeleteToolTip = "Ấn vào đây để xóa";
		public static string Infor_Edit = "Sửa";
		public static string Infor_DeleteSucc = "Đã xóa bản ghi";
		public static string Infor_InsertFailed = "Lỗi thêm bản ghi";
		public static string Infor_UpdateFailed = "Lỗi cập nhật bản ghi";
		public static string Infor_DeleteFailed = "Lỗi xóa bản ghi";
		public static string Infor_NotFound = "Không tìm thấy bản ghi";
		public static string Infor_IdNA = "Chưa có định danh";
		public static string Infor_EmptyValue = "Chưa có dữ liệu";
		public static string Infor_Exists = "Đã có bản ghi";
		public static string Infor_InvalidDateTime = "Sai định dạng ngày tháng";
		public static string Infor_NotEnoughCondition = "Chưa đủ điều kiện";
		public static string Infor_ChildParentSameId = "Trùng định danh với bản ghi cha";
		public static string Infor_NoPriviledge = "Chưa được phân quyền thực hiện thao tác này";

		public static string LogFilePath_ChangePass = "ChangePass";
		public static string LogFilePath_Deny = "Deny";
		public static string LogFilePath_OutOfTime = "OutOfTime";
		public static string LogFilePath_Exception = "Exception";
		public static string LogFilePath_NewLoginFailed = "NewLoginFailed";
		public static string LogFilePath_LoginFailed = "LoginFailed";

		public static string LogFilePath_NeedInsert = "NeedInsert";
		public static string LogFilePath_NeedCheck = "NeedCheck";
		public static string LogFilePath_NoData = "NoData";
		public static string LogFilePath_NotExists = "NotExists";
		public static string LogFilePath_Process = "Process";
		public static string LogFilePath_SevereError = "SevereError";
		public static string LogFilePath_Spy = "Spy";
		public static string LogFilePath_RunTime = "RunTime";
		public static string LogFilePath_Warning = "Warning";
		public const int BeginDateYear = 1900;
		public static char EncryptSeparator = '/';
		public static string MBTI = "MBTI";
		public const short ContentLength = 4000;
		//public static string EncryptPrefix = "Anhnx";

		public const byte AbsentTypes_Present = 1;
		public const byte AbsentTypes_NoPermission = 5;

		public const byte ActionStatus_Id_Active = 1;

		public const byte ActTypes_Insert = 1;
		public const byte ActTypes_Update = 2;
		public const byte ActTypes_Delete = 3;

		public const byte AssesmentMethods_Automatic = 1;
		public const byte AssesmentMethods_Manual = 2;

		public const byte AssesmentTypes_Diligent = 1;
		public const byte AssesmentTypes_Seminar = 2;
		public const byte AssesmentTypes_Exercise = 3;
		public const byte AssesmentTypes_GroupActivity = 4;
		public const byte AssesmentTypes_Practise = 5;
		public const byte AssesmentTypes_Experiment = 6;
		public const byte AssesmentTypes_MiddleExam = 7;
		public const byte AssesmentTypes_FinalExam = 8;
		public const byte AssesmentTypes_Talent = 9;
		public const byte AssesmentTypes_Precourse = 10;
		public const byte AssesmentTypes_Thema = 11;
		

		public const byte BookPartContentTypes_Teach = 1;
		public const byte BookPartContentTypes_Excersice = 3;

		public const byte CategoryStatus_Active = 1;

		public const byte CategoryTypes_Normal = 4;

		public const byte ChartTypes_MO = 1;
		public const byte ChartTypes_MT = 2;
		public const byte ChartTypes_CDR = 3;

		public const byte ClassExamOptionTypes_EndMark = 1;
		public const byte ClassExamOptionTypes_SumNeedMark = 2;
		public const byte ClassExamOptionTypes_MarkWithTimePriority = 3;
		public const byte ClassExamOptionTypes_RandomForUser = 4;
		public const byte ClassExamOptionTypes_DisplayCorrect = 5;
		public const byte ClassExamOptionTypes_NotMark = 6;
		public const byte ClassExamOptionTypes_PermitIps = 7;
		public const byte ClassExamOptionTypes_ManualMark = 8;

		public const byte ClassKinds_Id_InOrgan = 1;
		public const byte ClassKinds_Id_Online = 3;

		public const byte ClassOptionTypes_InviteEnroll = 1;
		public const byte ClassOptionTypes_NotCheckUserCode = 2;
		public const byte ClassOptionTypes_MaxSeatsPerCluster = 3;
		public const byte ClassOptionTypes_DisableMaterial = 4;
		public const byte ClassOptionTypes_Announcement = 5;
		public const byte ClassOptionTypes_RollupByUser = 6;

		public const byte ClassStatus_Active = 1;
		public const byte ClassStatus_Id_Active = 1;
		
		public const byte ClassUserStatus_Active = 1;

		public const byte CommissionTypes_Yes = 1;
		public const byte CommissionTypes_No = 2;

		public const byte CONTACTTYPENAME_EMAIL = 1;
		public const byte ContactTypes_Id_Email = 1;
		public const byte CONTACTTYPENAME_MOBILE_PHONE = 2;
		public const byte CONTACTTYPENAME_FIX_PHONE = 3;
		public const byte CONTACTTYPENAME_FAX = 4;
		public const byte CONTACTTYPENAME_ADDRESS = 5;

		public const byte CreatorKinds_Auto = 1;
		public const byte CreatorKinds_Manual = 2;
		public const byte CreatorKinds_System = 3;

		public const byte CuriculumnStatus_Active = 1;

		public const byte DataTypes_INT = 1;
		public const byte DataTypes_DATETIME = 2;
		public const byte DataTypes_String = 3;
		public const byte DataTypes_Combined = 4;
		public const byte DataTypes_IpAddress = 5;
		public const byte DataTypes_SubnetMask = 6;
		public const byte DataTypes_Logical = 7;

		public const byte LectureTypes_Id_Theory = 1;
		public const byte LectureTypes_Id_Practice= 2;
		public const byte LectureTypes_Id_Exercise = 3;
		public const byte LectureTypes_Id_SelfStudy = 4;
		public const byte LectureTypes_Id_OverAll = 5;

		


		public static string DECORATE_COLOR_INDICATORSIGN_Negative = "Purple";
		public static string DECORATE_COLOR_QUESTIONS_RELATED = "red";
		public static string DECORATE_COLOR_QUESTIONS_RELATING = "blue";

		

		public const byte DifficultLevels_Simple = 1;

		public const byte EDITORSTATUS_EDITING = 1;
		public const byte EDITORSTATUS_PENDING = 2;
		public const byte EDITORSTATUS_APPROVING = 3;
		public const byte EDITORSTATUS_APPROVED = 4;
		public const byte EDITORSTATUS_REJECTED = 5;
		public const byte EDITORSTATUS_NO_LONGER_USED = 255;

		public const string EducationForms_Name_Online = "Online";
		public const byte EducationForms_Id_OusideOrgan = 2;
		public const byte EducationForms_Id_Online = 3;

		public const byte EducationLevels_Id_University = 8;

		public const byte EnrollStatus_Progress = 1;
		public const byte EnrollStatus_Warning = 2;
		public const byte EnrollStatus_Expel = 3;
		public const byte EnrollStatus_NoReg = 4;
		public const byte EnrollStatus_Pending = 6;
		public const byte EnrollStatus_Id_Audit = 7;
		public const byte EnrollStatus_Completed = 255;

		public const byte ExamPatternOptionTypes_AllQuestionsForClassExams = 1;
		public const byte ExamPatternOptionTypes_MaxTimes = 2;

		public const byte ExamPatternQuestionStatus_Active = 1;

		public const byte ExamPatternStatus_Approved = 4;

		public const byte ExamStatus_Approved = 4;
		public const byte ExamStatus_NotFoundPattern = 9;

		public const byte FinalStatus_Progress = 1;
		public const byte FinalStatus_Completed = 2;
		public const byte FinalStatus_Failed = 3;

		public static string FORM_TITLE_NATIONALITIES_EN = "Nationality list";
		public static string FORM_TITLE_NATIONALITIES_VN = "Danh sách quốc tịch";

		public const byte GradeStatus_New = 1;
		public const byte GradeStatus_Progress = 2;
		public const byte GradeStatus_Finished = 3;

		public const byte GradeReasonTypes_Inherit = 10;
		public const byte GradeReasonTypes_NotConcise = 11;

		public const byte ImageTypes_SetTopImage = 1;
		public const byte ImageTypes_OriginalImage = 2;
		public const byte ImageTypes_Level2Image=3;

		public const byte ImportFileKinds_Xls = 1;

		public const byte IndicatorSigns_Positive = 1;

		public const byte Languages_English = 1;
		public const byte Languages_Vietnamese = 2;

		public const string Languages_Code_EN = "EN";
		public const string Languages_Code_VN = "VN";

		public const byte MarkStatus_Marking = 1;//"Đang đánh giá"
		public const byte MarkStatus_Marked = 2;//"Đã đánh giá"
		public const byte MarkStatus_AutoApproved = 3;//"Phê chuẩn tự động"
		public const byte MarkStatus_Approved = 4;//"Đã phê chuẩn"
		public const byte MarkStatus_Approving = 5;//"Đang phê duyệt"


		public const byte OrganLevels_Id_Faculty = 5;
		public const byte OrganLevels_Id_Groups = 8;

		public const byte OrganStatus_Id_Active = 1;
		
		

		public const byte OrganCuriculumnStatus_Active = 1;

		public const byte MESSAGETYPES_UNICODE = 30;

		public const string ParamTypes_ParamTypeCode_BreakAnswerSentences  = "Broadcast Address";
		public const string ParamTypes_ParamTypeCode_BroadcastAddress = "Broadcast Address";
		public const string ParamTypes_ParamTypeCode_CountNumberOfSubnet = "Count Number of subnet";
		public const string ParamTypes_ParamTypeCode_DefaultGateway = "Default gateway";
		public const string ParamTypes_ParamTypeCode_IP4 = "IP4";
		public const string ParamTypes_ParamTypeCode_IP4B2 = "IP4B2";
		public const string ParamTypes_ParamTypeCode_Ip_Interface = "Ip Interface";
		public const string ParamTypes_ParamTypeCode_HighValidIpAddress = "High Valid IP Address";
		public const string ParamTypes_ParamTypeCode_LowValidIpAddress ="Low Valid IP Address";
		public const string ParamTypes_ParamTypeCode_MessageList = "Message List";
		public const string ParamTypes_ParamTypeCode_MinimumBorowBits = "Minimum borow bits";
		public const string ParamTypes_ParamTypeCode_NetworkAddress = "Network Address";
		public const string ParamTypes_ParamTypeCode_NumberOfRecord = "Number of record";
		public const string ParamTypes_ParamTypeCode_NumberOfEvents = "Numbert of Events";
		public const string ParamTypes_ParamTypeCode_NumberOfProcesses = "Number of Processes";
		public const string ParamTypes_ParamTypeCode_Offset = "Offset";
		public const string ParamTypes_ParamTypeCode_ParamSeparatedComma = "Param separated comma";
		public const string ParamTypes_ParamTypeCode_PhysicalTime = "Physical Time";
		public const string ParamTypes_ParamTypeCode_SubnetMask = "Subnet Mask";
		public const string ParamTypes_ParamTypeCode_Subneting = "Subneting";
		public const string ParamTypes_ParamTypeCode_TimeStamp = "TimeStamp";
		public const string ParamTypes_ParamTypeCode_Usable = "Usable";
		public const string ParamTypes_ParamTypeCode_Use = "Use?";

		public const string ParamTypes_ParamTypeCode_ClientSendTime  = "Client Send Time";
		public const string ParamTypes_ParamTypeCode_ClientReceiveTime = "Client Receive Time";
		public const string ParamTypes_ParamTypeCode_ServerSendTime = "Server Send Time";
		public const string ParamTypes_ParamTypeCode_ServerReceiveTime = "Server Receive Time";
		public const string ParamTypes_ParamTypeCode_FileName = "FileName";
		                                                                  

		public const byte PrivacyTypes_Private = 1;
		public const byte PrivacyTypes_Group = 2;
		public const byte PrivacyTypes_Public = 3;

		public const byte ProcessStatusId_MtSentToTelco = 32;
		public const byte ProcessStatusId_CdrToFile = 36;
		public const byte ProcessStatusId_SentToSMSC = 40;
		
		public const byte ProgressStatus_AllowFinal = 1;
		public const byte ProgressStatus_NotAllowFinal = 2;
		
		public static string RANKNAME_INSTRUCTOR = "Instructor";
		
		public const byte Ranks_Director = 1;
		public const byte Ranks_AdminElearn = 2;
		public const byte Ranks_President = 3;
		public const byte Ranks_Instructor = 4;
		public const byte Ranks_VicePresident = 5;
		public const byte Ranks_Student = 6;
		public const byte Ranks_Head = 7;
		public const byte Ranks_DeptyHead = 8;
		public const byte Ranks_Dean = 9;
		public const byte Ranks_DeputyDean = 10;
		public const byte Ranks_Officer = 11;
		public const byte Ranks_Obsever = 12;
		public const byte Ranks_TechnicalSupport = 13;
		public const byte Ranks_Assement = 14;

		public const byte RelationTypes_Couple = 1;
		public const byte RelationTypes_Translate = 2;

		public const byte Remarks_Id_Normal = 1;
		public const byte Remarks_Id_Change = 3;
		public const byte Remarks_Id_Absent = 4;
		public const byte Remarks_Id_Summer = 5;

		

		public static string Ranks_Instructor_PreviousEn = "Classes had been teached";
		public static string Ranks_Instructor_PreviousVn = "Các lớp đã dạy";

		public static string Ranks_Student_PreviousEn = "Classes had been attended";
		public static string Ranks_Student_PreviousVn = "Các lớp đã tham gia";


		public const byte ReferenceTypes_Main = 1;

		public static string ROLENAME_ELEARN_ADMIN = "Elearn Admin";
		public static string ROLENAME_INSTRUCTOR = "Instructor";
		public static string ROLENAME_ORGAN_ADMIN = "Organ Admin";
		public static string ROLENAME_SYSTEM_ADMIN = "System admin";
		public static string ROLENAME_Tester = "Tester";
		public static string ROLENAME_VasDealer = "Vas Dealer";

		public const byte RollupForms_Id_Manual = 1;
		public const byte RollupForms_Id_Online = 2;
		public const string Rooms_Code_Online = "Online";

		public static string QUESTION_GROUP_TALENT = "Ta";
		public static string QUESTION_GROUP_PERSONALGROUPS = "P";
		public static string QUESTION_GROUP_VALUES = "V";

		public const byte QuestionStatus_Approved = 4;
		
		public const byte QuestionTypes_Quiz = 1;
		public const byte QuestionTypes_Foreword = 2;
		public const byte QuestionTypes_Matrix = 3;
		public const byte QuestionTypes_NetOpenLab = 4;
		public const byte QuestionTypes_VectorTimestamp = 5;
		public const byte QuestionTypes_LamportTimestamp = 6;
		public const byte QuestionTypes_CristianAlgorithm = 7;
		public const byte QuestionTypes_BerkeleyAlgorithm = 8;
		public const byte QuestionTypes_WirelessVote = 9;
		public const byte QuestionTypes_Summary = 10;		                  
		public const byte QuestionTypes_PositiveIntToBinary = 11;
		public const byte QuestionTypes_NegativeIntToBinary = 12;
		public const byte QuestionTypes_PositiveBinaryToInt = 13;
		public const byte QuestionTypes_NegativeBinaryToInt = 14;
		public const byte QuestionTypes_BitOperators = 15;
		public const byte QuestionTypes_ByzantineAlgorithm = 16;
		public const byte QuestionTypes_Permutations = 17;
		public const byte QuestionTypes_NTP = 18;
		public const byte QuestionTypes_FDS = 19;//Phụ thuộc hàm
		public const byte QuestionTypes_AverageAlgorithm = 22;
		public const byte QuestionTypes_CodingNtp = 23;

		public const byte ShiftTypes_Id_Evening = 3;

		public const byte ScheduleStatus_Id_New = 1;
		public const byte ScheduleStatus_Id_Approve = 4;
		public const byte ScheduleStatus_Id_NotUse = 255;

		public const byte Status_Id_Active = 1;
		
		public const byte Status_Id_Finished = 3;

		public const byte UserExamStatus_Marked = 1;
		public const byte UserExamStatus_Progress = 2;
		public const byte UserExamStatus_Submited = 3;
		public const byte UserExamStatus_Forced = 4;
		public const byte UserExamStatus_Violated = 5;
		public const byte UserExamStatus_New = 7;
		
		public const byte UserExamStatus_Proving = 8;
		public const byte UserExamStatus_Review = 254;
		public const byte UserExamStatus_ReMark = 255;
		
		public const byte UserStatus_New = 1;
		public const byte UserStatus_Registered = 2;
		public const byte UserStatus_Cancel = 3;
		public const byte UserStatus_UseAnother = 4;
		public const byte UserStatus_WaitToEnrol = 5;
		
		
		public const byte SubjectTypes_Questions = 1;
		public const byte SubjectTypes_BookPartContents = 3;
		public const byte SUBJECTTYPE_ARTICLES = 4;
		public const byte SubjectTypes_DiscussThemes = 5;
		public const byte SUBJECTTYPE_ARTICLESLEAD = 6;
		public const byte SubjectTypes_UserHomeExercises = 7;
		public const byte SubjectTypes_Answers = 8;
		public const byte SubjectTypes_Explains = 9;

		public const byte TrainingTypes_Id_CQ = 1;

		public const byte TopologyTypes_Id_Theatre = 1;
		public const byte TopologyTypes_Id_Stripe = 4;
		public const byte TopologyTypes_Id_Free = 255;

		public const byte TransactionForms_Bank = 1;

		

		public const byte UsageStatus_Id_Normal = 1;
		
		public const byte UserLogStatus_Id_Sucess = 1;
		public const byte UserLogStatus_Id_DisabledAccount = 2;
		public const byte UserLogStatus_Id_InvalidPassword = 3;
		public const byte UserLogStatus_Id_ResetPassword = 8;
		
		public const byte UserOrganStatus_Active = 1;

		public const byte UserStatus_Active = 1;
		public const byte UserStatus_NoActive = 2;

		public const int Users_DefaultId = 48;

		public const byte ValidateStatus_Self = 1;
		public const byte ValidateStatus_Invalid = 2;
		public const byte ValidateStatus_Id_Validated = 255;

		public const byte ValueMethodes_Predefined = 1;
		public const byte ValueMethodes_Random = 2;

		public const byte VasSubscriberStatus_Active = 1;
		public const byte VasSubscriberStatus_NotExists = 2;
		public const byte VasSubscriberStatus_InfoNA = 3;
		public const byte VasSubscriberStatus_Expired = 250;
		public const byte VasSubscriberStatus_Unreg = 251;
		public const byte VasSubscriberStatus_WaitConfirm = 252;
		public const byte VasSubscriberStatus_Rejected = 253;
		public const byte VasSubscriberStatus_RegFailed = 254;
		public const byte VasSubscriberStatus_NotActive = 255;
		
		





		public static string CODE_VIETNAMESE = "VN";
		public static string CODE_ENGLISH = "EN";
		public static string UNICODE = "UNICODE";
		public static string UTF8 = "UTF8";

		public static string ERRORID_SUCCESS = "1";
		public static string ERRORDESC_SUCCESS = "Success";
		public static string VNERRORDESC_SUCCESS = "Thành công";

		public static string ERRORID_DENY_IP = "100";
		public static string ERRORDESC_DENY_IP = "Denied IP";
		public static string VNERRORDESC_DENY_IP = "Từ chối địa chỉ IP";

		public static string ERRORID_SUBSCRIBER_EXISTED = "101";
		public static string ERRORDESC_SUBSCRIBER_EXISTED = "Subscriber existed";
		public static string VNERRORDESC_SUBSCRIBER_EXISTED = "Thuê bao đã tồn tại";

		public static string ERRORID_SYSTEM_EXCEPTION = "110";
		public static string ERRORDESC_SYSTEM_EXCEPTION = "System exeption";
		public static string VNERRORDESC_SYSTEM_EXCEPTION = "Lỗi hệ thống";

		public static string ERRORID_PACKAGE_NOT_EXISTS = "114";
		public static string ERRORDESC_PACKAGE_NOT_EXISTS = "Package does not exist";
		public static string VNERRORDESC_PACKAGE_NOT_EXISTS = "Gói dịch vụ không tồn tại";

		public static string ERRORID_EMPTY_RESULT = "119";
		public static string ERRORDESC_EMPTY_RESULT = "Empty result";
		public static string VNERRORDESC_EMPTY_RESULT = "Kết quả xử lý rỗng";

		public static string ERRORID_EMPTY_PACKAGENAME = "120";
		public static string ERRORDESC_EMPTY_PACKAGENAME = "Empty package name";
		public static string VNERRORDESC_EMPTY_PACKAGENAME = "Tên gói cước rỗng";

		public static string ERRORID_EMPTY_INVALID_PHONE = "130";
		public static string ERRORDESC_EMPTY_INVALID_PHONE = "Invalid phone number";
		public static string VNERRORDESC_EMPTY_INVALID_PHONE = "Số máy không hợp lệ";

		public static string ERRORID_EMPTY_INVALID_PREFIXID = "140";
		public static string ERRORDESC_EMPTY_INVALID_PREFIXID = "Invalid PrefixId";
		public static string VNERRORDESC_EMPTY_INVALID_PREFIXID = "Đầu số không hợp lệ";

		public static string ERRORID_EMPTY_INVALID_MESSAGEOUT = "150";
		public static string ERRORDESC_EMPTY_INVALID_MESSAGEOUT = "Empty Message Out";
		public static string VNERRORDESC_EMPTY_INVALID_MESSAGEOUT = "Chưa có nội dung";

		public static string ERRORID_SESSION_NOT_EXISTS = "160";
		public static string ERRORDESC_SESSION_NOT_EXISTS = "Session not exists";
		public static string VNERRORDESC_SESSION_NOT_EXISTS = "Phiên dịch vụ không tồn tại";

		public static string ERRORID_CONTENTTYPEID_NOT_AVAILABLE = "170";
		public static string ERRORDESC_CONTENTTYPEID_NOT_AVAILABLE = "ContentTypeId not available";
		public static string VNERRORDESC_CONTENTTYPEID_NOT_AVAILABLE = "Chưa có định danh nội dung";

		public static string ERRORID_EMPTY_CONTENTTYPENAME = "171";
		public static string ERRORDESC_EMPTY_CONTENTTYPENAME = "Empty ContentTypeName";
		public static string VNERRORDESC_EMPTY_CONTENTTYPENAME = "Chưa có mã nội dung";

		public static string ERRORID_INVALID_TELCO = "190";
		public static string ERRORDESC_INVALID_TELCO = "Invalid Telco";
		public static string VNERRORDESC_INVALID_TELCO = "Mạng không hợp lệ";

		public static string ERRORID_EMPTY_SERVICECODE = "200";
		public static string ERRORDESC_EMPTY_SERVICECODE = "Empty ServiceCode";
		public static string VNERRORDESC_EMPTY_SERVICECODE = "Mã dich vụ rỗng";

		public static string ERRORID_SERVICE_NOT_FOUND = "201";
		public static string ERRORDESC_SERVICE_NOT_FOUND = "Service not found";
		public static string VNERRORDESC_SERVICE_NOT_FOUND = "Không tìm thấy dịch vụ";

		public static string ERRORID_EMPTY_USERNAME = "500";
		public static string ERRORDESC_EMPTY_USERNAME = "Empty User Name";
		public static string VNERRORDESC_EMPTY_USERNAME = "Tên truy nhập rỗng";


		public static string ERRORID_EMPTY_USERPASS = "501";
		public static string ERRORDESC_EMPTY_USERPASS = "Empty User Password";
		public static string VNERRORDESC_EMPTY_USERPASS = "Mật khẩu rỗng";

		public static string ERRORID_EMPTY_INVALND_USERNAME = "503";
		public static string ERRORDESC_EMPTY_INVALND_USERNAME = "Invalid User or Password";
		public static string VNERRORDESC_EMPTY_INVALND_USERNAME = "Sai tên hoặc mật khẩu";

		public static string ERRORID_INVALND_FULLNAME = "504";
		public static string ERRORDESC_INVALND_FULLNAME = "Invalid fullname";
		public static string VNERRORDESC_INVALND_FULLNAME = "Họ và tên không hợp lệ";


		public static string ERRORID_EMPTY_ADVPARTNERNAME = "510";
		public static string ERRORDESC_EMPTY_ADVPARTNERNAME = "Empty business name";
		public static string VNERRORDESC_EMPTY_ADVPARTNERNAME = "Tên đối tác kinh doanh rỗng";

		public static string ERRORID_INVALID_DATACARD = "600";
		public static string ERRORDESC_INVALID_DATACARD = "Invalid Datacard";
		public static string VNERRORDESC_INVALID_DATACARD = "Số thẻ không hợp lệ";

		public static string ERRORID_INVALID_CARDSERIE = "601";
		public static string ERRORDESC_INVALID_CARDSERIE = "Invalid Card Serie";
		public static string VNERRORDESC_INVALID_CARDSERIE = "Serie thẻ không hợp lệ";

		public static string ERRORID_CANNOT_LOGIN = "995";
		public static string ERRORDESC_CANNOT_LOGIN = "Cannot login";
		public static string VNERRORDESC_CANNOT_LOGIN = "Cannot login";

		public static string ERRORID_OUT_OF_SERVICE = "999";
		public static string ERRORDESC_OUT_OF_SERVICE = "Out of service";
		public static string VNERRORDESC_OUT_OF_SERVICE = "Chưa phục vụ";

		public static string ERRORID_EMPTY_DEALER_NA = "0";
		public static string ERRORDESC_EMPTY_DEALER_NA = "Daler not available";
		public static string VNERRORDESC_EMPTY_DEALER_NA = "Chưa có đại lý";

		public static string ERRORID_CONTACTTYPE_INVALID = "1001";
		public static string ERRORDESC_CONTACTTYPE_INVALID = "Invalid Contact Type";
		public static string VNERRORDESC_CONTACTTYPE_INVALID = "Kênh liên lạc không hợp lệ";

		public static string ERRORID_NOT_MEMBER = "1002";
		public static string ERRORDESC_NOT_MEMBER = "Not member";
		public static string VNERRORDESC_NOT_MEMBER = "Bạn chưa là thành viên để sử dụng dịch vụ bình chọn, vui lòng đăng ký bằng cách gửi lại nội dung \"DK\" tới hệ thống iVote";

		public static string ERRORID_LOW_BALANCE = "1003";
		public static string ERRORDESC_LOW_BALANCE = "Low balance";
		public static string VNERRORDESC_LOW_BALANCE = "Số dư tài khoản của bạn là #Rep1 VND, không đủ để sử dụng dịch vụ bình chọn, vui lòng nạp tiền và tài khoản để sử dụng dịch vụ. Thông tin nạp tiền cụ thể như sau:"+Environment.NewLine
			+"Tên tài khoản: Công ty…."+Environment.NewLine
      +"Số tài khoản:…."+Environment.NewLine
      +"Mở tại CN Ngân hàng Vietcombank Thăng Long"+Environment.NewLine
      +"Nội dung: [Ten Facebook hoặc Skype] nạp tiền vào TK, và nhắn tin \"Đã chuyển tiền, số tiền bằng số\" cho chúng tôi sau khi chuyển tiền";
		
		public static string ERRORID_VALUE_INVALID = "1004";
		public static string ERRORDESC_VALUE_INVALID = "Invalid value";
		public static string VNERRORDESC_VALUE_INVALID = "Giá trị không hợp lệ";

		public static string ERRORID_NOT_RECEIVED = "1005";
		public static string ERRORDESC_NOT_RECEIVED = "Not received";
		public static string VNERRORDESC_NOT_RECEIVED = "Hiện chúng tôi chưa nhận được TT từ NH của Quý khách. Chúng tôi sẽ thông báo tới Quý khách ngay khi nhận được báo có của Ngân hàng. Nhắn 0 để được Trợ giúp (Đồng thời cập nhật tình trạng vào CSDL theo dõi thanh toán)";

	}
}
