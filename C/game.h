void strengthenTaoistSkills() {

  while (true) {

    HDC hdc = GetDC(NULL);
    if (hdc) {
      //是否选定怪物
      if (GetPixel(hdc, 240, 96) == 0xADC8DD &&
          GetPixel(hdc, 240, 97) == 0xADC8DD) {
        //  召唤白虎
        if (GetPixel(hdc, 501, 595) == 0xFFF2FC) {
          keybd_event(0x32, 3, 0, 0);
          keybd_event(0x32, 3, KEYEVENTF_KEYUP, 0);
          Sleep(1000);
          keybd_event(0x33, 4, 0, 0);
          keybd_event(0x33, 4, KEYEVENTF_KEYUP, 0);
          Sleep(500);
          keybd_event(0x32, 3, 0, 0);
          keybd_event(0x32, 3, KEYEVENTF_KEYUP, 0);
          Sleep(500);
        }
      }

      ReleaseDC(NULL, hdc);
    }
    Sleep(1000);
  }
}

// 彩虹岛抓龙虾
void catchingLobster() {
  // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-keybd_event
  // printf("%d\n", MapVirtualKey(0x31, 0));
  // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-mapvirtualkeya

  while (true) {
    keybd_event(0x31, 2, 0, 0);
    keybd_event(0x31, 2, KEYEVENTF_KEYUP, 0);
    Sleep(1000);
    HDC hdc = GetDC(NULL);
    if (hdc) {
      //是否选定怪物
      if (GetPixel(hdc, 451, 591) == 0xFFFFFF) {

        Press(0x32);
        Sleep(1000);
      } else {
        Press(0x31);
        Sleep(1000);
      }

      ReleaseDC(NULL, hdc);
    }
    Sleep(1000);
  }
}

void strengthenMageSkills() {
  int count = 0;
  while (true) {
    keybd_event(0x32, 3, 0, 0);
    keybd_event(0x32, 3, KEYEVENTF_KEYUP, 0);
    Sleep(1000);
    if (count > 10) {
      keybd_event(0x33, 4, 0, 0);
      keybd_event(0x33, 4, KEYEVENTF_KEYUP, 0);
      Sleep(1000);
      keybd_event(0x31, 2, 0, 0);
      keybd_event(0x31, 2, KEYEVENTF_KEYUP, 0);
      Sleep(500);
      count = 0;
    }
    count++;
  }
}
void plantFlowers() {
  POINT p;
  GetCursorPos(&p);
  for (size_t i = 0; i < 5; i++) {
    for (size_t j = 0; j < 3; j++) {
      Click(p.x + i * 100, p.y + j * 100);
      Sleep(1000);
    }
  }
}