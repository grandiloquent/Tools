#include <Windows.h>
#include <stdbool.h>
#include <stdio.h>
#include <stdlib.h>

void Drag(int x, int y) {
  int maxX = GetSystemMetrics(SM_CXSCREEN),
      maxY = GetSystemMetrics(SM_CYSCREEN);
  // int x = 1191, y =202;
  double factorX = 65536.0 / maxX, factorY = 65536.0 / maxY;

  INPUT ip;

  ZeroMemory(&ip, sizeof(ip));

  ip.type = INPUT_MOUSE;

  ip.mi.mouseData = 0;
  ip.mi.dx = x * factorX;
  ip.mi.dy = y * factorY;

  ip.mi.dwFlags = MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTDOWN;

  SendInput(1, &ip, sizeof(ip));

  ip.mi.dwFlags = MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_MOVE;

  SendInput(1, &ip, sizeof(ip));

  ip.mi.dwFlags = MOUSEEVENTF_ABSOLUTE | MOUSEEVENTF_LEFTUP;

  SendInput(1, &ip, sizeof(ip));
}

bool SetText(const char *str) {
  // GetForegroundWindow()
  if (!OpenClipboard(0)) {
    printf("ERROR");
    return FALSE;
  }
  EmptyClipboard();
  const size_t len = strlen(str) + 1;
  HGLOBAL hglb = GlobalAlloc(GMEM_MOVEABLE, len);
  memcpy(GlobalLock(hglb), str, len);
  GlobalUnlock(hglb);
  // EmptyClipboard();
  HANDLE res = SetClipboardData(CF_TEXT, hglb);
  bool isSet = res ? TRUE : FALSE;
  if (!isSet) {
    GlobalFree(hglb);
  }
  CloseClipboard();
  return isSet;
}

POINT p;
typedef struct Thread {
  HANDLE handle;
  DWORD dwThreadId;
  BOOL status;
} thread;

#define RegisterHotKeyWithoutModifier(X, Y)                                    \
  if (RegisterHotKey(NULL, X, 0, X)) {                                         \
    printf("成功快捷键：%c = %d = 0x%0X, %s\n", MapVirtualKeyA(X, 2), X, X,    \
           Y);                                                                 \
  }
#define HandleHotKeyWithThread(X, Y, Z)                                        \
  if (msg.wParam == X) {                                                       \
    GetCursorPos(&p);                                                          \
    if (!hWnd) {                                                               \
      hWnd = WindowFromPoint(p);                                               \
    }                                                                          \
    if (!threads[Y].handle) {                                                  \
      threads[Y].handle =                                                      \
          CreateThread(NULL, 0, Z, &hWnd, 0, &threads[Y].dwThreadId);          \
      threads[Y].status = TRUE;                                                \
      printf("创建线程 %d = %d\n", Y, threads[Y].dwThreadId);                  \
    } else {                                                                   \
      if (threads[Y].status) {                                                 \
        SuspendThread(threads[Y].handle);                                      \
        threads[Y].status = FALSE;                                             \
        printf("暂停线程 %d = %d\n", Y, threads[Y].dwThreadId);                \
      } else {                                                                 \
        ResumeThread(threads[Y].handle);                                       \
        threads[Y].status = TRUE;                                              \
        printf("重启线程 %d = %d\n", Y, threads[Y].dwThreadId);                \
      }                                                                        \
    }                                                                          \
  }

void Click(int x, int y) {
  SetCursorPos(x, y);
  mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
  Sleep(150);
  mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
}

void RightClick(int x, int y) {
  SetCursorPos(x, y);
  mouse_event(MOUSEEVENTF_RIGHTDOWN, 0, 0, 0, 0);
  Sleep(150);
  mouse_event(MOUSEEVENTF_RIGHTUP, 0, 0, 0, 0);
}

INPUT createScanCodeEvent(int vkCode, bool isDown) {
  INPUT input = {0};
  input.type = INPUT_KEYBOARD;
  input.ki.wVk = vkCode;
  input.ki.wScan = MapVirtualKeyEx(vkCode, 0, GetKeyboardLayout(0));
  input.ki.dwFlags = (isDown ? 0 : KEYEVENTF_KEYUP) | KEYEVENTF_SCANCODE;
  input.ki.time = 0;
  input.ki.dwExtraInfo = 0;
  return input;
}

void SendKeyBackground(HWND hWnd, WORD wVk) {
  PostMessage(hWnd, WM_KEYDOWN, wVk, 0);

  Sleep(150);
  // PostMessage(hWnd, WM_CHAR, wVk,0);

  PostMessage(hWnd, WM_KEYUP, wVk, 0);
}

void BackgroundMouseLeftClick(HWND hWnd, int x, int y) {
  PostMessage(hWnd, WM_LBUTTONDOWN, 0, MAKELPARAM(x, y));
  Sleep(100);
  PostMessage(hWnd, WM_LBUTTONUP, 0, MAKELPARAM(x, y));
}

void SendKeyWithAlt(HWND hWnd, UINT vk) {
  PostMessage(hWnd, WM_SYSKEYDOWN, 0x12, 0x60380001);
  Sleep(100);
  PostMessage(hWnd, WM_SYSKEYUP, 0x12, 0xC0380001);
  Sleep(100);
  // PostMessage(hWnd, WM_SYSKEYDOWN, 0x12, 0x60380001);
  // Sleep(10);
  PostMessage(hWnd, WM_SYSKEYDOWN, vk, 0);
  Sleep(100);
  PostMessage(hWnd, WM_SYSKEYUP, vk, 0);
  Sleep(100);
  // PostMessage(hWnd, WM_KEYUP, 0x12, 0);
}

void Press(int vkey) {
  /*INPUT ip;
  ip.type = INPUT_KEYBOARD;
  ip.ki.time = 0;
  ip.ki.dwFlags = KEYEVENTF_UNICODE;
  ip.ki.wScan = VK_RETURN; //VK_RETURN is the code of Return key
  ip.ki.wVk = 0;

  ip.ki.dwExtraInfo = 0;
  SendInput(1, &ip, sizeof(INPUT));*/
  INPUT input;
  // WORD vkey = VK_RETURN; // see link below
  input.type = INPUT_KEYBOARD;
  // input.ki.wScan = MapVirtualKey(vkey, MAPVK_VK_TO_VSC);

  input.ki.wVk = vkey;
  // input.ki.dwFlags = 0; // there is no KEYEVENTF_KEYDOWN
  SendInput(1, &input, sizeof(INPUT));

  input.ki.dwFlags = KEYEVENTF_KEYUP;
  SendInput(1, &input, sizeof(INPUT));
}

//
void SendText(const char *str) {
  POINT point;
  GetCursorPos(&point);
  HWND hwnd = WindowFromPoint(point);
  size_t len = strlen(str);
  for (int i = 0; i < len; ++i) {
    SHORT c = VkKeyScan(str[i]); // tolower(VkKeyScan(str[i]));
    SendMessage(hwnd, WM_KEYDOWN, c, 0);
    SendMessage(hwnd, WM_CHAR, c, 0);
    SendMessage(hwnd, WM_KEYUP, c, 0);
    Sleep(150);
  }
}

void CtrlShiftClick(int x, int y) {
  INPUT ctrl;
  ctrl.type = INPUT_KEYBOARD;
  ctrl.ki.wVk = VK_CONTROL;
  SendInput(1, &ctrl, sizeof(INPUT));

  INPUT shift;
  shift.type = INPUT_KEYBOARD;
  shift.ki.wVk = VK_SHIFT;
  SendInput(1, &shift, sizeof(INPUT));
  Sleep(150);
  Click(x, y);
  Sleep(150);
  shift.ki.dwFlags = KEYEVENTF_KEYUP;
  SendInput(1, &shift, sizeof(INPUT));
  ctrl.ki.dwFlags = KEYEVENTF_KEYUP;
  SendInput(1, &ctrl, sizeof(INPUT));
}

void CtrlClick(int x, int y) {
  INPUT ctrl;
  ctrl.type = INPUT_KEYBOARD;
  ctrl.ki.wVk = VK_CONTROL;
  SendInput(1, &ctrl, sizeof(INPUT));

  Sleep(150);
  Click(x, y);
  Sleep(150);
  ctrl.ki.dwFlags = KEYEVENTF_KEYUP;
  SendInput(1, &ctrl, sizeof(INPUT));
}

void CtrlPress(int c) {
  INPUT input[4];
  memset(input, 0, 4 * sizeof(input[0]));
  input[0].type = INPUT_KEYBOARD;
  input[1].type = INPUT_KEYBOARD;
  input[0].ki.wVk = VK_CONTROL;
  // https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
  // HKL currentKBL = GetKeyboardLayout(0);
  // printf("%d",VkKeyScanEx(']', currentKBL ));

  input[1].ki.wVk = c; // towupper(c);
  input[2] = input[1];
  input[3] = input[0];
  input[2].ki.dwFlags = KEYEVENTF_KEYUP;
  input[3].ki.dwFlags = KEYEVENTF_KEYUP;
  SendInput(4, input, sizeof(input[0]));
}

void ShiftPress(int c) {
  INPUT input[4];
  memset(input, 0, 4 * sizeof(input[0]));
  input[0].type = INPUT_KEYBOARD;
  input[1].type = INPUT_KEYBOARD;
  input[0].ki.wVk = VK_SHIFT;
  // https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes
  // HKL currentKBL = GetKeyboardLayout(0);
  // printf("%d",VkKeyScanEx(']', currentKBL ));

  input[1].ki.wVk = c; // towupper(c);
  input[2] = input[1];
  input[3] = input[0];
  input[2].ki.dwFlags = KEYEVENTF_KEYUP;
  input[3].ki.dwFlags = KEYEVENTF_KEYUP;
  SendInput(4, input, sizeof(input[0]));
}

void DoubleClick(int x, int y) {
  const double XSCALEFACTOR = 65535 / (GetSystemMetrics(SM_CXSCREEN) - 1);
  const double YSCALEFACTOR = 65535 / (GetSystemMetrics(SM_CYSCREEN) - 1);

  POINT cursorPos;
  GetCursorPos(&cursorPos);

  double cx = cursorPos.x * XSCALEFACTOR;
  double cy = cursorPos.y * YSCALEFACTOR;

  double nx = x * XSCALEFACTOR;
  double ny = y * YSCALEFACTOR;

  INPUT Input = {0};
  Input.type = INPUT_MOUSE;

  Input.mi.dx = (LONG)nx;
  Input.mi.dy = (LONG)ny;

  Input.mi.dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE |
                     MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP;

  SendInput(1, &Input, sizeof(INPUT));
  SendInput(1, &Input, sizeof(INPUT));

  Input.mi.dx = (LONG)cx;
  Input.mi.dy = (LONG)cy;

  Input.mi.dwFlags = MOUSEEVENTF_MOVE | MOUSEEVENTF_ABSOLUTE;

  SendInput(1, &Input, sizeof(INPUT));
}
