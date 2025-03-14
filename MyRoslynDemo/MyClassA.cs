using System;

namespace MyRoslynDemo;

public class MyClassA
{
    public static void DoSomethingA()
    {
        // In real code, we'd do something fancy.
        // For demo, let's call MyClassB.
        MyClassB.DoSomethingB();
    }
}
