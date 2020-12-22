using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//自定义的attribute需要继承System.Attribute,然后我们可以使用c#的预定义特性去注明此attribute的应用范围。如果我们自定义的attribute名为UI，那么我们的自定义的attribute类名则应该为UIAttribute。
public enum UILayer
{
    Default,
    Secondary,
    Popup,
    TipAndWarning,
}

[AttributeUsage(AttributeTargets.Class)]
public class UIAttribute : Attribute
{
    public UILayer Layer { get; set; }
}

//如上所示，我们定义了一个attribute，UI，并且指明了应用范围只能在类上应用。我们用这个attribute来注明不同的View类的Layer属性，使用代码如下：
public class View
{
    public UILayer Layer;
    
    public void GetView()
    {
        Debug.Log("This is View");
    }
}

[UI(Layer = UILayer.Default)]
public class LoginView: View
{
    public void GetView()
    {
        Debug.Log("This is LoginView");
    }
}

[UI(Layer = UILayer.Popup)]
public class AlertInfoView : View
{
    public void GetView()
    {
        Debug.Log("This is AlertInfoView");
    }
}

public class UseOfAttribute : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Test();
    }

    public View CreateView(Type type)
    {
        UIAttribute classAttribute = (UIAttribute) Attribute.GetCustomAttribute(type, typeof(UIAttribute));
        View view = Activator.CreateInstance(type) as View;
        view.Layer = classAttribute.Layer;

        return view;
    }

    public void Test()
    {
        View login = CreateView(typeof(LoginView));
        View alert = CreateView(typeof(AlertInfoView));
        Debug.Log(login.Layer);
        Debug.Log(alert.Layer);
    }
}
