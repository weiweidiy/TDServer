// See https://aka.ms/new-console-template for more information
using JFrame;




ITimerUtils timerUtils = new JFrameTimerUtils();

timerUtils.Regist(1f, -1, () => { Console.WriteLine("111"); })

Console.WriteLine("Hello, World!");