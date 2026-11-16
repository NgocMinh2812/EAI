; ============================================================================
; BỘ CÀI ENGLISHAI — đọc và biên dịch bằng Inno Setup 6.3 trở lên
; ============================================================================
; Cách tạo bộ cài:
;   1. Trong Visual Studio, publish project GUI bằng profile DemoWinX64.
;   2. Mở file EAI.iss này bằng Inno Setup Compiler và chọn Build > Compile (F9).
;   3. Lấy file EnglishAI-Setup-<phiên bản>.exe trong thư mục installer\output.
;
; Hành vi khi cài đặt:
;   - Chưa có app: cài mới cho tài khoản Windows đang chạy bộ cài.
;   - Đã có cùng phiên bản: hỏi xác nhận cài lại để thay thế file chương trình.
;   - Đã có bản cũ hơn: hỏi xác nhận nâng cấp vào thư mục đã cài trước đó.
;   - Đã có bản mới hơn: chặn cài đè bằng bộ cài cũ.
;   - Không chạy trình gỡ bản cũ trước khi nâng cấp; ghi đè các file trùng tên.
;   - File cũ không còn nằm trong bản publish có thể vẫn còn trên máy.
;     Khi cần bỏ một file lỗi thời, thêm đúng tên file đó vào [InstallDelete].
;   - Gỡ cài đặt: xóa file do bộ cài quản lý và shortcut, không tác động Azure SQL.
;     File người dùng tự tạo ngoài danh sách cài đặt thường được giữ lại.
;   - Chặn thư mục hệ thống, thư mục tạm và thư mục chứa sẵn file không thuộc app.
;   - Khi nâng cấp, bắt buộc dùng thư mục của bản đã đăng ký với Windows.
;   - Nếu thông tin bản cũ bị hỏng, yêu cầu xử lý bản cũ trước khi cài tiếp.
;   - Trước khi gỡ, kiểm tra tiến trình đúng đường dẫn GUI.exe của bản đang gỡ.
;
; Chú ý khi phát hành bản tiếp theo:
;   - Tăng MyAppVersion, ví dụ từ 1.0.0 thành 1.0.1.
;   - Giữ nguyên AppId và UninstallKey để nhận ra bản đã cài trước đó.
;   - Publish lại trước khi Compile để đóng gói đúng code mới.
;   - Bộ cài không kiểm tra Internet, firewall Azure hay thông tin đăng nhập.
;     App cần Internet để sử dụng Azure SQL và Backend AI.
;
; Cú pháp chú thích: dùng dấu ; ở các phần cấu hình, dùng // trong [Code].
; ============================================================================

; Tên hiển thị trên bộ cài, shortcut và danh sách ứng dụng của Windows.
#define MyAppName "EnglishAI"

; Phiên bản của bộ cài; đồng thời dùng để so sánh với bản đã cài trên máy.
#define MyAppVersion "1.0.3"

; SourcePath là thư mục chứa file .iss này (installer).
; Bản publish nằm ở EAI\publish\win-x64 trong project; không dùng bin\Debug.
#define PublishDir SourcePath + "..\EAI\publish\win-x64"

; Kiểm tra ngay khi Compile, không phải lúc người dùng chạy bộ cài.
; Thiếu GUI.exe nghĩa là chưa publish hoặc PublishDir đang trỏ sai thư mục.
#if !FileExists(PublishDir + "\GUI.exe")
  #error "Publish DemoWinX64 first: GUI.exe is missing."
#endif
; coreclr.dll là một thành phần runtime .NET đi kèm bản Self-contained.
; Bản này đóng gói runtime để máy đích không cần cài .NET riêng.
#if !FileExists(PublishDir + "\coreclr.dll")
  #error "A self-contained win-x64 publish is required (coreclr.dll missing)."
#endif
; File này cho .NET biết cấu hình runtime cần dùng để chạy GUI.
#if !FileExists(PublishDir + "\GUI.runtimeconfig.json")
  #error "Publish output is incomplete: GUI.runtimeconfig.json missing."
#endif
; Bản publish dạng nhiều file phải có cả mã ứng dụng và danh sách dependency.
#if !FileExists(PublishDir + "\GUI.dll") || !FileExists(PublishDir + "\GUI.deps.json")
  #error "Incomplete publish: GUI.dll or GUI.deps.json is missing."
#endif
; Không chỉ kiểm tra coreclr: cần cả bộ nạp .NET và thư viện giao diện WPF.
#if !FileExists(PublishDir + "\hostfxr.dll") || !FileExists(PublishDir + "\hostpolicy.dll") || !FileExists(PublishDir + "\PresentationFramework.dll")
  #error "Incomplete self-contained WPF runtime. Publish DemoWinX64 again."
#endif
; Các lớp nghiệp vụ/dữ liệu bắt buộc để app có thể chạy sau khi cài.
#if !FileExists(PublishDir + "\DAL.dll") || !FileExists(PublishDir + "\BUS.dll") || !FileExists(PublishDir + "\DTO.dll") || !FileExists(PublishDir + "\Microsoft.Data.SqlClient.dll")
  #error "Incomplete application dependencies: DAL/BUS/DTO/SqlClient is missing."
#endif

[Setup]
; Mã định danh cố định của app. Dấu {{ ở đầu là cách viết dấu { trong Inno Setup.
; Không tạo AppId mới mỗi lần phát hành, nếu không Windows sẽ nhận thành app khác.
AppId={{4828D617-6CF2-4626-97DD-5C5CF5DD150E}

; Thông tin hiển thị và phiên bản trong Properties của file bộ cài.
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher=EnglishAI
VersionInfoVersion={#MyAppVersion}

; Mặc định cài vào AppData\Local\Programs\EnglishAI của người dùng hiện tại.
; lowest: không yêu cầu quyền quản trị; các tài khoản Windows khác không tự có app.
DefaultDirName={localappdata}\Programs\EnglishAI
DefaultGroupName={#MyAppName}
PrivilegesRequired=lowest

; Chỉ nhận Windows 10 build 17763 (1809) trở lên.
; Đây là giới hạn của bộ cài, không thay thế việc kiểm thử trên từng bản Windows.
MinVersion=10.0.17763

; Nhớ thư mục, nhóm Start Menu và lựa chọn shortcut của lần cài trước.
; Với bản cài đã nhận diện được, trang chọn thư mục sẽ được bỏ qua tự động.
UsePreviousAppDir=yes
UsePreviousGroup=yes
UsePreviousTasks=yes
DisableDirPage=auto
; Không hiện trang chọn nhóm Start Menu, dùng DefaultGroupName ở trên.
DisableProgramGroupPage=yes

; Tránh cài vào ổ mạng, đường dẫn UNC (\\máy-chủ\thư-mục) hoặc gốc ổ đĩa.
AllowNetworkDrive=no
AllowUNCPath=no
AllowRootDirectory=no
; Inno Setup tự tính dung lượng theo các file được đóng gói và cảnh báo khi thiếu.
; Không đặt một mức GB cố định vì dung lượng bản publish có thể thay đổi.
; Hiện trang tổng hợp để người dùng kiểm tra thư mục trước khi bấm Install.
DisableReadyPage=no

; Ngăn hai tiến trình bộ cài EnglishAI chạy đồng thời.
; Mutex này dành cho bộ cài, không giới hạn số cửa sổ của chính ứng dụng EAI.
SetupMutex=EnglishAI_Setup_4828D617

; Dùng Windows Restart Manager phát hiện app đang giữ file EXE/DLL cần thay thế.
; Khi cài tương tác, bộ cài đề nghị đóng app; không dùng chế độ cưỡng ép force.
; Người dùng nên lưu công việc và đóng EAI trước khi tiếp tục.
CloseApplications=yes
CloseApplicationsFilter=*.exe,*.dll
; Không tự mở lại các app vừa đóng; cuối bộ cài có tùy chọn Open EnglishAI.
RestartApplications=no

; Tạo trình gỡ cài đặt và mục EnglishAI trong danh sách ứng dụng Windows.
; append: nối thông tin cài mới vào nhật ký gỡ cài đặt khi cài đè/nâng cấp.
Uninstallable=yes
UninstallLogMode=append
UninstallDisplayName={#MyAppName}
; Ghi nhật ký xử lý bộ cài trong thư mục tạm để hỗ trợ tìm lỗi cài đặt.
SetupLogging=yes
; Khi gỡ từ danh sách ứng dụng Windows, cũng ghi log để tìm nguyên nhân nếu lỗi.
UninstallLogging=yes

; Gói publish là win-x64: yêu cầu Windows có khả năng chạy ứng dụng x64.
; ARM64 có thể được nhận nếu hệ điều hành hỗ trợ giả lập x64; cần kiểm thử riêng.
ArchitecturesAllowed=x64compatible
ArchitecturesInstallIn64BitMode=x64compatible

; Đường dẫn tương đối tính từ thư mục chứa file .iss.
; Kết quả: installer\output\EnglishAI-Setup-<phiên bản>.exe.
OutputDir=output
OutputBaseFilename=EnglishAI-Setup-{#MyAppVersion}

; Icon của bộ cài lấy từ source; icon gỡ cài đặt lấy từ GUI.exe đã cài.
SetupIconFile=..\EAI\GUI\Assets\app_icon.ico
UninstallDisplayIcon={app}\GUI.exe

; Nén LZMA2, nén chung các file để giảm dung lượng; dùng giao diện wizard hiện đại.
Compression=lzma2
SolidCompression=yes
WizardStyle=modern

[Tasks]
; Cho người dùng chọn tạo shortcut ngoài Desktop; mặc định không chọn ở lần đầu.
Name: "desktopicon"; Description: "Create a desktop shortcut"; Flags: unchecked

[Files]
; {app} là thư mục cài đặt thực tế được Inno Setup chọn cho lần chạy này.
; Chép toàn bộ bản publish, bao gồm DLL, runtime .NET và các thư mục tài nguyên.
; Excludes: loại file debug, log và một số file cấu hình riêng khỏi gói cài.
; Danh sách này chỉ lọc file rời; không loại chuỗi kết nối đã biên dịch trong DLL.
;
; ignoreversion: luôn thay file trùng tên bằng file trong bộ cài.
;   Việc chặn hạ phiên bản được thực hiện trong InitializeSetup bên dưới.
; recursesubdirs: chép cả file trong thư mục con.
; createallsubdirs: tạo cả thư mục con rỗng không bị loại trừ.
;
; Không xóa đệ quy {app}: app có thể lưu tài liệu/audio người dùng ngay tại đây.
; File tự tạo không có trong gói cài được giữ lại; file trùng đường dẫn với file
; trong gói cài sẽ bị ghi đè, nên không lưu dữ liệu cá nhân dưới tên file chương trình.
; Chưa có [InstallDelete] hay [UninstallDelete] để xóa thêm dữ liệu ngoài danh sách.
Source: "{#PublishDir}\*"; DestDir: "{app}"; Excludes: "*.pdb,*.log,.env,.env.*,secrets.json,appsettings.secrets.json,apiminh.json,api.json,logs\*,Uploads\*"; Flags: ignoreversion recursesubdirs createallsubdirs

[Icons]
; Shortcut Start Menu được tạo cùng app; WorkingDir đặt thư mục làm việc là {app}.
Name: "{group}\{#MyAppName}"; Filename: "{app}\GUI.exe"; WorkingDir: "{app}"
; Shortcut Desktop chỉ được tạo nếu tác vụ desktopicon đang được chọn.
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\GUI.exe"; WorkingDir: "{app}"; Tasks: desktopicon

[Run]
; Hiện lựa chọn mở app ở trang hoàn tất; không chờ app thoát mới kết thúc bộ cài.
; skipifsilent: không tự mở GUI khi triển khai bằng chế độ cài im lặng.
Filename: "{app}\GUI.exe"; Description: "Open EnglishAI"; Flags: nowait postinstall skipifsilent

[Code]
// Phần Pascal Script xử lý những điều kiện cần thêm ngoài cấu hình có sẵn.
const
  // Inno Setup ghi thông tin app đã cài vào khóa Registry này.
  // Phần GUID phải trùng AppId; hậu tố _is1 là quy ước khóa gỡ cài của Inno Setup.
  UninstallKey = 'Software\Microsoft\Windows\CurrentVersion\Uninstall\{4828D617-6CF2-4626-97DD-5C5CF5DD150E}_is1';

// Chuẩn hóa đường dẫn trước khi so sánh: xử lý dấu chấm, dấu gạch chéo cuối.
// Luôn so sánh có dấu \ cuối để C:\App không bị nhầm là cha của C:\Application.
function NormalizeDir(const Value: String): String;
begin
  Result := AddBackslash(ExpandFileName(Trim(Value)));
end;

function SameDir(const LeftDir, RightDir: String): Boolean;
begin
  // Biến môi trường có thể rỗng; không biến chuỗi rỗng thành thư mục hiện hành.
  Result := False;
  if (Trim(LeftDir) = '') or (Trim(RightDir) = '') then Exit;
  Result := CompareText(NormalizeDir(LeftDir), NormalizeDir(RightDir)) = 0;
end;

function IsInsideDir(const ChildDir, ParentDir: String): Boolean;
begin
  Result := Pos(Lowercase(NormalizeDir(ParentDir)),
    Lowercase(NormalizeDir(ChildDir))) = 1;
end;

// Chỉ nhận đường dẫn tuyệt đối đến ổ đĩa, dạng C:\...; không nhận UNC/tương đối.
function IsLocalAbsoluteDir(const Value: String): Boolean;
begin
  Result := False;
  if Length(Value) < 3 then Exit;
  Result := (Pos(Uppercase(Copy(Value, 1, 1)), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ') > 0)
    and (Copy(Value, 2, 2) = ':\');
end;

// Không ghi đè vào thư mục có sẵn dữ liệu mà chưa được nhận diện là bản EAI cũ.
function HasDirectoryEntries(const Dir: String): Boolean;
var
  FindRec: TFindRec;
begin
  Result := False;
  if FindFirst(AddBackslash(Dir) + '*', FindRec) then
  begin
    try
      repeat
        if (FindRec.Name <> '.') and (FindRec.Name <> '..') then
        begin
          Result := True;
          Break;
        end;
      until not FindNext(FindRec);
    finally
      FindClose(FindRec);
    end;
  end;
end;

// Đọc lại thông tin mỗi lần kiểm tra, kể cả ngay trước khi sao chép file.
// Trả về chuỗi rỗng khi hợp lệ; trả về thông báo lỗi khi cần dừng cài đặt.
function ReadInstalledState(var Installed: Boolean;
  var InstalledVersion, InstalledDir: String): String;
var
  OldVersion, NewVersion: Int64;
begin
  Result := '';
  Installed := False;
  InstalledVersion := '';
  InstalledDir := '';

  // Không trộn bản cài cho toàn máy với bản cài riêng cho một tài khoản Windows.
  if RegKeyExists(HKLM32, UninstallKey) or RegKeyExists(HKLM64, UninstallKey) then
  begin
    Result := 'Đã có EnglishAI được cài cho toàn bộ máy. Hãy gỡ bản đó trong ' +
      'Settings > Apps trước khi dùng bộ cài dành cho người dùng hiện tại.';
    Exit;
  end;

  if not StrToVersion('{#MyAppVersion}', NewVersion) then
  begin
    Result := 'Phiên bản bộ cài không hợp lệ. Cần sửa MyAppVersion và biên dịch lại.';
    Exit;
  end;

  Installed := RegKeyExists(HKCU, UninstallKey);
  if not Installed then Exit;

  // Registry tồn tại nhưng thiếu/sai phiên bản: không tự đoán rồi cài đè.
  if not RegQueryStringValue(HKCU, UninstallKey, 'DisplayVersion', InstalledVersion) then
    Result := 'Không đọc được phiên bản EnglishAI đã cài.'
  else if not StrToVersion(InstalledVersion, OldVersion) then
    Result := 'Thông tin phiên bản EnglishAI đã cài không hợp lệ.';
  if Result <> '' then
  begin
    Result := Result + ' Hãy gỡ bản cũ trong Settings > Apps rồi cài lại.';
    Exit;
  end;

  if ComparePackedVersion(OldVersion, NewVersion) > 0 then
  begin
    Result := 'Máy đã có EnglishAI ' + InstalledVersion +
      ', mới hơn bộ cài {#MyAppVersion}. Hãy dùng bộ cài cùng hoặc mới hơn bản đã cài.';
    Exit;
  end;

  if not RegQueryStringValue(HKCU, UninstallKey, 'InstallLocation', InstalledDir) then
    InstalledDir := '';
  if not IsLocalAbsoluteDir(InstalledDir) then
    Result := 'Thông tin thư mục cài EnglishAI bị thiếu hoặc không hợp lệ. ' +
      'Hãy gỡ bản cũ trong Settings > Apps rồi cài lại.';
end;

// Kiểm tra vị trí trước khi sao chép. Không tạo/xóa file để thử thư mục ở bước này.
function ValidateInstallDir(const Candidate: String): String;
var
  Installed: Boolean;
  InstalledVersion, InstalledDir, Target: String;
begin
  Result := ReadInstalledState(Installed, InstalledVersion, InstalledDir);
  if Result <> '' then Exit;

  if not IsLocalAbsoluteDir(Trim(Candidate)) then
  begin
    Result := 'Hãy chọn đường dẫn tuyệt đối trên ổ đĩa cục bộ, ví dụ C:\Apps\EnglishAI.';
    Exit;
  end;
  Target := NormalizeDir(Candidate);

  if SameDir(Target, ExtractFileDrive(Target) + '\') then
    Result := 'Không được cài trực tiếp vào gốc ổ đĩa. Hãy chọn thư mục riêng cho EnglishAI.'
  else if IsInsideDir(Target, ExpandConstant('{win}')) or
    IsInsideDir(Target, ExpandConstant('{commonpf}')) or
    IsInsideDir(Target, ExpandConstant('{commonpf32}')) or
    IsInsideDir(Target, ExpandConstant('{commonappdata}')) then
    Result := 'Bộ cài dành cho người dùng hiện tại không cài vào thư mục hệ thống. ' +
      'Hãy dùng thư mục mặc định trong AppData\Local\Programs\EnglishAI.'
  else if IsInsideDir(Target, ExpandConstant('{tmp}')) then
    Result := 'Không cài EnglishAI vào thư mục tạm vì Windows có thể dọn xóa thư mục đó.'
  // Inno Setup không có hằng {userprofile}; đọc biến môi trường của máy đang cài.
  else if SameDir(Target, GetEnv('USERPROFILE')) or
    SameDir(Target, ExpandConstant('{localappdata}')) or
    SameDir(Target, ExpandConstant('{userappdata}')) or
    SameDir(Target, ExpandConstant('{userdesktop}')) or
    SameDir(Target, ExpandConstant('{userdocs}')) then
    Result := 'Hãy tạo thư mục riêng tên EnglishAI, không cài trực tiếp vào thư mục cá nhân này.'
  else if SameDir(Target, ExpandConstant('{src}')) then
    Result := 'Thư mục cài app phải khác thư mục đang chứa file bộ cài.'
  else if Installed and not SameDir(Target, InstalledDir) then
    Result := 'Để cài lại hoặc nâng cấp, phải dùng thư mục của bản đã cài: ' +
      InstalledDir + #13#10 + 'Nếu muốn chuyển vị trí, hãy gỡ bản cũ trước.'
  else if not Installed then
  begin
    if DirExists(Target) and HasDirectoryEntries(Target) then
      Result := 'Thư mục đã có file nhưng không được nhận diện là EnglishAI đã cài. ' +
        'Hãy chọn thư mục trống để tránh ghi đè dữ liệu hoặc một bản portable.';
  end;
end;

// Khởi động bộ cài: hỏi cài lại/nâng cấp, chặn thông tin cũ hỏng và hạ phiên bản.
function InitializeSetup(): Boolean;
var
  Installed: Boolean;
  InstalledVersion, InstalledDir, ErrorText: String;
begin
  ErrorText := ReadInstalledState(Installed, InstalledVersion, InstalledDir);
  Result := ErrorText = '';
  if not Result then
  begin
    Log(ErrorText);
    SuppressibleMsgBox(ErrorText, mbError, MB_OK, IDOK);
    Exit;
  end;

  if Installed then
    // Cài silent vẫn tiếp tục nếu không có lỗi, không treo chờ xác nhận.
    Result := SuppressibleMsgBox('Máy đã cài EnglishAI ' + InstalledVersion + '.' + #13#10 +
      'Tiếp tục cài lại hoặc nâng cấp lên {#MyAppVersion}?' + #13#10 +
      'File chương trình sẽ được thay thế tại: ' + InstalledDir + #13#10 +
      'Dữ liệu trên Azure SQL không bị xóa.' + #13#10 +
      'Hãy lưu công việc và đóng EnglishAI trước khi tiếp tục.',
      mbConfirmation, MB_YESNO, IDYES) = IDYES;
end;

// Kiểm tra cả ở trang chọn thư mục lẫn trang xác nhận, áp dụng cho cài silent.
function NextButtonClick(CurPageID: Integer): Boolean;
var
  ErrorText: String;
begin
  Result := True;
  if (CurPageID = wpSelectDir) or (CurPageID = wpReady) then
  begin
    ErrorText := ValidateInstallDir(WizardDirValue);
    Result := ErrorText = '';
    if not Result then
    begin
      Log(ErrorText);
      SuppressibleMsgBox(ErrorText, mbError, MB_OK, IDOK);
    end;
  end;
end;

// Kiểm tra lần cuối ngay trước khi ghi file, kể cả khi chạy với /DIR=... hoặc silent.
// Sau bước này Inno Setup tiếp tục kiểm tra quyền ghi, dung lượng và file đang dùng.
function PrepareToInstall(var NeedsRestart: Boolean): String;
begin
  Result := ValidateInstallDir(WizardDirValue);
end;

// WMI chỉ đọc trạng thái tiến trình; không kill app và không chỉ xét tên GUI.exe.
// Kết quả: 0 = đã đóng; 1 = app đúng đường dẫn vẫn chạy; -1 = không kiểm tra được.
function GetAppRunningState(const AppDir: String): Integer;
var
  Locator, Services, Processes, Process: Variant;
  Index: Integer;
  ExePath: String;
  HasUnknownProcess: Boolean;
begin
  Result := -1;
  HasUnknownProcess := False;
  try
    Locator := CreateOleObject('WbemScripting.SWbemLocator');
    Services := Locator.ConnectServer('', 'root\CIMV2');
    Processes := Services.ExecQuery('SELECT ExecutablePath FROM Win32_Process WHERE Name = ''GUI.exe''');
    for Index := 0 to Processes.Count - 1 do
    begin
      Process := Processes.ItemIndex(Index);
      // Một tiến trình có thể không cho đọc đường dẫn (ví dụ chạy bằng quyền khác).
      // Khi đó không được tự kết luận rằng app đã đóng.
      if VarIsNull(Process.ExecutablePath) then
        HasUnknownProcess := True
      else
      begin
        ExePath := Process.ExecutablePath;
        if ExePath = '' then
          HasUnknownProcess := True
        else if CompareText(ExpandFileName(ExePath),
          ExpandFileName(AddBackslash(AppDir) + 'GUI.exe')) = 0 then
        begin
          Result := 1;
          Exit;
        end;
      end;
    end;
    if not HasUnknownProcess then Result := 0;
  except
    Log('Không kiểm tra được tiến trình EnglishAI: ' + GetExceptionMessage);
  end;
end;

// Không gỡ khi app đang chạy; cho phép đóng app rồi Retry mà không mất dữ liệu dở.
// Nếu WMI lỗi, dừng và ghi log; không coi lỗi kiểm tra là app đã đóng.
// Bản gỡ im lặng trả về thất bại thay vì lặp vô hạn hoặc tự kill app.
function InitializeUninstall(): Boolean;
var
  RunningState: Integer;
  ErrorText: String;
begin
  Result := False;
  if SuppressibleMsgBox('Bạn muốn gỡ EnglishAI?' + #13#10 +
    'File chương trình và shortcut sẽ được xóa.' + #13#10 +
    'Dữ liệu trên Azure SQL không bị xóa. File tự tạo ngoài danh sách cài được giữ lại.',
    mbConfirmation, MB_YESNO, IDYES) <> IDYES then Exit;

  while True do
  begin
    RunningState := GetAppRunningState(ExpandConstant('{app}'));
    if RunningState = 0 then
    begin
      Result := True;
      Exit;
    end;
    if RunningState = 1 then
      ErrorText := 'EnglishAI vẫn đang mở. Hãy lưu công việc, đóng app rồi chọn Retry để gỡ.'
    else
      ErrorText := 'Windows chưa kiểm tra được trạng thái EnglishAI qua WMI. ' +
        'Hãy đóng các tiến trình GUI.exe còn mở hoặc kiểm tra dịch vụ ' +
        'Windows Management Instrumentation rồi chọn Retry. ' +
        'Bộ gỡ chưa xóa file nào.';
    Log(ErrorText);
    if UninstallSilent then Exit;
    if SuppressibleMsgBox(ErrorText, mbError, MB_RETRYCANCEL, IDCANCEL) <> IDRETRY then Exit;
  end;
end;
