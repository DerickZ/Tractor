; �ýű�ʹ�� HM VNISEdit �ű��༭���򵼲���

; ��װ�����ʼ���峣��
!define PRODUCT_NAME "��������ս"
!define PRODUCT_VERSION "1.2.0.357"
!define PRODUCT_PUBLISHER "������"
!define PRODUCT_WEB_SITE "http://smallnest.googlepages.com"
!define PRODUCT_UNINST_KEY "Software\Microsoft\Windows\CurrentVersion\Uninstall\${PRODUCT_NAME}"
!define PRODUCT_UNINST_ROOT_KEY "HKLM"
!define PRODUCT_STARTMENU_REGVAL "NSIS:StartMenuDir"

SetCompressor lzma

;-------��� .NET Framework�İ汾
!include "DotNET2.nsh"
!include "LogicLib.nsh"
!define DOTNET_VERSION "2.0.50727"


; ------ MUI �ִ����涨�� (1.67 �汾���ϼ���) ------
!include "MUI.nsh"


; MUI Ԥ���峣��
!define MUI_ABORTWARNING
!define MUI_ICON "Tractor-install.ico"
!define MUI_UNICON "Tractor-uninstall.ico"

;ͼƬ
!define MUI_HEADERIMAGE
!define MUI_HEADERIMAGE_BITMAP  "Header.bmp"
!define MUI_WELCOMEFINISHPAGE_BITMAP  "Wizard.bmp"

;����
!define MUI_LANGDLL_REGISTRY_ROOT "${PRODUCT_UNINST_ROOT_KEY}"
!define MUI_LANGDLL_REGISTRY_KEY "${PRODUCT_UNINST_KEY}"
!define MUI_LANGDLL_REGISTRY_VALUENAME "NSIS:Language"


; ��ӭҳ��
!insertmacro MUI_PAGE_WELCOME
; ���Э��ҳ��
!insertmacro MUI_PAGE_LICENSE "license.txt"
; ��װĿ¼ѡ��ҳ��
!insertmacro MUI_PAGE_DIRECTORY
; ��ʼ�˵�����ҳ��
var ICONS_GROUP
!define MUI_STARTMENUPAGE_NODISABLE
!define MUI_STARTMENUPAGE_DEFAULTFOLDER "��������ս"
!define MUI_STARTMENUPAGE_REGISTRY_ROOT "${PRODUCT_UNINST_ROOT_KEY}"
!define MUI_STARTMENUPAGE_REGISTRY_KEY "${PRODUCT_UNINST_KEY}"
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME "${PRODUCT_STARTMENU_REGVAL}"
!insertmacro MUI_PAGE_STARTMENU Application $ICONS_GROUP
; ��װ����ҳ��
!insertmacro MUI_PAGE_INSTFILES
; ��װ���ҳ��
!insertmacro MUI_PAGE_FINISH

; ��װж�ع���ҳ��
!insertmacro MUI_UNPAGE_INSTFILES

; ��װ�����������������
!insertmacro MUI_LANGUAGE "SimpChinese"

; ��װԤ�ͷ��ļ�
!insertmacro MUI_RESERVEFILE_INSTALLOPTIONS
; ------ MUI �ִ����涨����� ------

Name "${PRODUCT_NAME} ${PRODUCT_VERSION}"
OutFile "Tractor.exe"
InstallDir "$PROGRAMFILES\��������ս"
ShowInstDetails show
ShowUnInstDetails show

BrandingText "Kuaff�Ƽ�  http://smallnest.googlepages.com"

Section "MainSection" SEC01
  
  
  
  SetOutPath "$INSTDIR"
  SetOverwrite ifnewer
  File /r "..\release\*.*"

  !insertmacro CheckDotNET
  Delete "$INSTDIR\dotnetfx.exe"
  
; ������ʼ�˵���ݷ�ʽ
  !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
  CreateDirectory "$SMPROGRAMS\$ICONS_GROUP"
  CreateShortCut "$SMPROGRAMS\$ICONS_GROUP\��������ս.lnk" "$INSTDIR\Tractor.exe"
  !insertmacro MUI_STARTMENU_WRITE_END
SectionEnd

Section -AdditionalIcons
  !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
  CreateShortCut "$SMPROGRAMS\$ICONS_GROUP\ж����������ս.lnk" "$INSTDIR\uninst.exe"
  !insertmacro MUI_STARTMENU_WRITE_END
SectionEnd

Section -Post
  WriteUninstaller "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayName" "$(^Name)"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "UninstallString" "$INSTDIR\uninst.exe"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "DisplayVersion" "${PRODUCT_VERSION}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "URLInfoAbout" "${PRODUCT_WEB_SITE}"
  WriteRegStr ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}" "Publisher" "${PRODUCT_PUBLISHER}"
SectionEnd

/******************************
 *  �����ǰ�װ�����ж�ز���  *
 ******************************/

Section Uninstall
  !insertmacro MUI_STARTMENU_GETFOLDER "Application" $ICONS_GROUP
  Delete "$INSTDIR\uninst.exe"

  Delete "$SMPROGRAMS\$ICONS_GROUP\ж����������ս.lnk"
  Delete "$SMPROGRAMS\$ICONS_GROUP\��������ս.lnk"

  RMDir "$SMPROGRAMS\$ICONS_GROUP"

  RMDir /r "$INSTDIR\plugins"
  RMDir /r "$INSTDIR\music"
  RMDir /r "$INSTDIR\cards"
  RMDir /r "$INSTDIR\cardbackimgs"

  RMDir /r "$INSTDIR"

  DeleteRegKey ${PRODUCT_UNINST_ROOT_KEY} "${PRODUCT_UNINST_KEY}"
  SetAutoClose true
SectionEnd

#-- ���� NSIS �ű��༭�������� Function ���α�������� Section ����֮���д���Ա��ⰲװ�������δ��Ԥ֪�����⡣--#

Function un.onInit
  MessageBox MB_ICONQUESTION|MB_YESNO|MB_DEFBUTTON2 "��ȷʵҪ��ȫ�Ƴ� $(^Name) ���������е������" IDYES +2
  Abort
FunctionEnd

Function un.onUninstSuccess
  HideWindow
  MessageBox MB_ICONINFORMATION|MB_OK "$(^Name) �ѳɹ��ش���ļ�����Ƴ���"
FunctionEnd
