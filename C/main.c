// -fexec-charset=GBK -lGdi32
/* Linux-like double-linked list implementation */

#include "input.h"
#include "list.h"
#include "game.h"

/*void DoubleClick(int x, int y) {
        SetCursorPos(x, y);
        mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);

        Sleep(150);

        mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
}*/
struct listitem {
  POINT p;
  struct list_head list;
};

DWORD WINAPI Strategy(LPVOID lpParam) {

  HWND hWnd = *(HWND *)lpParam;
  while (1) {
    Press(0x52);
    Sleep(1000);
    Press(0x52);
    Sleep(4000);
    Press(0x52);
    Sleep(1000);
    Press(0x52);
    Sleep(4000);
    Press(0x36);
    Sleep(1000);
    Press(0x36);
    Sleep(3000);
  }
  return 0;
}

struct list_head testlist;

DWORD WINAPI Strategy1() {
  struct listitem *item = NULL;
  item = (struct listitem *)malloc(sizeof(*item));
  POINT pp;
  GetCursorPos(&pp);
  item->p = pp;
  list_add_tail(&item->list, &testlist);

  //    HDC hdc = GetDC(NULL);
  //
  //    for (int i =393; i < 979; ++i) {
  //        for (int j = 197; j < 512; ++j) {
  //            printf("%dx%d\n",i,j);
  //            if (GetPixel(hdc, i, j) == 0x9ECCDF
  //              ) {
  //                Click(i, j);
  //                Sleep(6000);
  //
  //            }
  //        }
  //    }
  //    ReleaseDC(NULL, hdc);

  return 0;
}

DWORD WINAPI Strategy2(LPVOID lpParam) {
  struct listitem *item = NULL, *is = NULL;
  list_for_each_entry_safe(item, is, &testlist, list) {
    Click(item->p.x, item->p.y);
    Sleep(6000);
  };
  list_for_each_entry_safe(item, is, &testlist, list) {
    list_del(&item->list);
    free(item);
  }
  return 0;
}

void method1() {
  while (true) {
    HDC hdc = GetDC(NULL);
    if (hdc) {
      if (GetPixel(hdc, 675, 497) == 0xE1DAC8 &&
          GetPixel(hdc, 697, 491) == 0xE1DAC8) {
        Click(675, 497);
        Sleep(1000);
      }

      if (GetPixel(hdc, 1187, 36) == 0xADC8DD &&
          GetPixel(hdc, 1188, 35) == 0xADC8DD) {
        Click(939, 93);
        Sleep(1000);
        Click(526, 109);
        Sleep(1000);
        Click(492, 574);
        Sleep(1000);
        Click(225, 326);
        Sleep(1000);
        Click(372, 550);
        Sleep(1000);
        Click(1072, 72);
        Sleep(1000);
      }
      ReleaseDC(NULL, hdc);
    }
    Sleep(1000);
  }
}

void method2() {
  int count = 0;
  while (true) {
    HDC hdc = GetDC(NULL);
    if (hdc) {
      if (GetPixel(hdc, 605, 372) == 0x34C25F &&
          GetPixel(hdc, 605, 373) == 0x34C25F) {
        // 进入
        Click(549, 430);
        Sleep(1000);
      } else if (GetPixel(hdc, 1185, 35) == 0xADC8DD &&
                 GetPixel(hdc, 1197, 39) == 0xADC8DD) {
        Click(645, 54);
        Sleep(1000);
        Click(268, 371);
        Sleep(1000);
        Click(590, 598);
        Sleep(1000);
        // 竞技
        count = 0;
      }

      if (GetPixel(hdc, 1163, 31) == 0xADC8DD &&
          GetPixel(hdc, 1204, 41) == 0xADC8DD) {
        // 进入
        Click(1261, 137);
        Sleep(1000);
        Click(1114, 214);
        Sleep(1000);
        Click(534, 346);
        Sleep(1000);
        Click(1073, 74);
        Sleep(1000);
        Click(1260, 137);
        Sleep(10000);
        count++;
        if (count > 30) {
          Click(1158, 522);
          Sleep(1000);
        }
      }

      ReleaseDC(NULL, hdc);
    }
    Sleep(1000);
  }
}

DWORD WINAPI Strategy5(LPVOID lpParam) {
  //    while (true) {
  //        Click(569, 582);
  //        Sleep(5000);
  //        Click(985, 347);
  //        Sleep(5000);
  //        Click(845, 581);
  //        Sleep(10000);
  //    }
  // method2();
  strengthenMageSkills();
  return 0;
}

DWORD WINAPI Strategy6(LPVOID lpParam) {
  //   while (true) {
  //     HDC hdc = GetDC(NULL);
  //     if (hdc) {

  //       if (GetPixel(hdc, 240, 93) == 0xADC8DD &&
  //           GetPixel(hdc, 240, 94) == 0xADC8DD) {
  //         Click(868, 612);
  //         Sleep(1000);
  //         Click(868, 612);
  //         Sleep(1000);
  //         Click(409, 615);
  //         Sleep(3000);
  //       }
  //       ReleaseDC(NULL, hdc);
  //     }
  //     Sleep(1000);
  strengthenTaoistSkills();
  //  catchingLobster();
  //        Click(659,613);
  //        Sleep(1000);
  //        Click(659,613);
  //        Sleep(5000);
  //        Click(949,190);
  //        Sleep(1000);

  return 0;
}
DWORD WINAPI Strategy4(LPVOID lpParam) {
  plantFlowers();
  return 0;
}
int main(void) {
  INIT_LIST_HEAD(&testlist);

  // https://docs.microsoft.com/en-us/windows/win32/inputdev/virtual-key-codes

  RegisterHotKeyWithoutModifier(VK_F11, "强化"); // I
  RegisterHotKeyWithoutModifier(VK_F10, "强化"); // I
  RegisterHotKeyWithoutModifier(VK_F9, "强化");  // I
  RegisterHotKeyWithoutModifier(VK_F8, "强化");
  RegisterHotKeyWithoutModifier(VK_F7, "F7 强化法师");
  RegisterHotKeyWithoutModifier(VK_F6, "F6 强化道士技能");
  RegisterHotKeyWithoutModifier(VK_F4, "F4 种花");

  thread threads[4] = {{0}, {0}, {0}, {0}};
  HWND hWnd = 0;
  MSG msg = {0};

  while (GetMessage(&msg, NULL, 0, 0) != 0) {
    if (msg.message != WM_HOTKEY)
      continue;
    HandleHotKeyWithThread(VK_F7, 2, Strategy5);
    HandleHotKeyWithThread(VK_F6, 3, Strategy6);
    HandleHotKeyWithThread(VK_F11, 0, Strategy);
    HandleHotKeyWithThread(VK_F4, 0, Strategy4);

    if (msg.wParam == VK_F9) {

      if (!threads[1].handle) {
        threads[1].handle =
            CreateThread(NULL, 0, Strategy2, &hWnd, 0, &threads[1].dwThreadId);
        threads[1].status = TRUE;
        printf("创建线程 %d = %d\n", 1, threads[1].dwThreadId);
      } else {
        if (threads[1].status) {
          // https://docs.microsoft.com/en-us/windows/win32/api/processthreadsapi/nf-processthreadsapi-suspendthread
          //
          TerminateThread(threads[1].handle, 0);
          struct listitem *item = NULL, *is = NULL;
          list_for_each_entry_safe(item, is, &testlist, list) {
            list_del(&item->list);
            free(item);
          }
          printf("终止线程 %d = %d\n", 1, threads[1].dwThreadId);
          threads[1].status = FALSE;

        } else {
          threads[1].handle = CreateThread(NULL, 0, Strategy2, &hWnd, 0,
                                           &threads[1].dwThreadId);
          threads[1].status = TRUE;
        }
      }
    }
    if (msg.wParam == VK_F10) {
      Strategy1();
    } else if (msg.wParam == VK_F8) {
      /*POINT pv = {0};
      GetCursorPos(&pv);
      HANDLE w;
      w = WindowFromPoint(pv);
      CaptureAnImage(w);
      */
      // https://docs.microsoft.com/en-us/windows/win32/api/winuser/nf-winuser-setclipboarddata
    }
    //		if (msg.wParam == 0x73) {
    //			system("ffmpeg -hide_banner -rtbufsize 150M -f gdigrab
    //-framerate 30 -offset_x 0 -offset_y 0 -video_size 1280x720 -draw_mouse 1
    // -i desktop -c:v libx264 -r 30 -preset ultrafast -tune zerolatency -crf 28
    //-pix_fmt
    // yuv420p -movflags +faststart -y
    //\"C:\\Users\\Administrator\\Desktop\\1.mp4");
    //
    //		}
  }
  return 0;
}
