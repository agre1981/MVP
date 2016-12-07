# MVP Pattern Recommendations for C# (.NET)

## 1. Overview MVP


## 2. Class Diagram

![Class Diagram](https://github.com/agre1981/MVP/blob/master/docs/img/mvp-classdiagram.png)

## 3. Sequencing diagram
*	**Presenter** communicates with **view** directly
  
![Sequence Diagram] (https://github.com/agre1981/MVP/blob/master/docs/img/mvp-seqdiagram-1.png)
  
*	**Presenter** communicates with **view** within **Model (using binding)**
  
![Sequence Diagram] (https://github.com/agre1981/MVP/blob/master/docs/img/mvp-seqdiagram-2.png)
  
## 4. MVP binding:
  * **Inside view**
```
// Constructor
public View()
{
  var presenter = new Presenter(this);
}

// view creation
var view = new View();
view.Show();
```
  * **Outside View** (for multiple Presenter implementations)
```
var view = new View();
var presenter = new Presenter(view);
view.Show();
```
## 5.	Presenter initialization
```
Presenter(IView view)
{
	this.view = view;
	this.model = new Model();
	view.FormLoadEvent += this.Load;
	view.ButtonClickEvent += this.Click;
}
```
## 6. Firing events in View
* Simplifying firing:
```
form.Load += (s, args) =>{ FormLoadEvent(); };
button.Click += (s, args) =>{ ButtonClickEvent(args); };
```
* Using Commands:
todo

## 7. Handling events in Presenter:
```
void view_FieldValidating(CancelEventHandler e)
{
	e.Cancel = /* expression */;
}
```
## 8.	Presenter interaction with view.
```
void Presenter.DoSomething()
{
  view.BindModel(model);
  view.AskUser();
  view.MakeBeep();
}
```
## 9.	Using Services.
* Class diagram

![Sequence Diagram] (https://github.com/agre1981/MVP/blob/master/docs/img/mvp-service.png)

* Binding:
```
var presenter = new Presenter(view, new Service());
```

## 10. Encapsulation.

To hide internal implementation of component inside assembly declare interface as **"internal"** and implement it **explicitly** in view (do it if you really need it).
```
internal interface IView
{
	void f();
	event Action<string> act;
}
public class View : IView
{
	private object objectLock = new Object(); 
	internal event Action<string> act;

	void IView.f() { }

	event Action<string> IView.act
  {
	   add { lock (objectLock) { act += value; } }
	   remove { lock (objectLock) { act -= value; } }
  }
}
```
## 11. Unit Testing with Moq 
* Raising events:
```
var viewMock = new Mock<IView>();
var presenter = new Presenter(viewMock.Object);
viewMock.Raise (m =>m.LoadEvent += null, EventArgs.Empty);
Assert.AreEqual( … );
```
*	Check pulling/calling of view/service methods:
```
/* view methods invocation verification */
viewMock.Verify(v =>v.MakeBeep(), Times.Once()); 
viewMock.Verify(v =>v.AskUser(), Times.Once()); 
/* service methods invocation verification */
viewMock.Verify(s =>s.SaveData(), Times.Once()); 
```
* Use Friend Assemblies to test “internal” members (attribute InternalsVisibleTo). See details on https://msdn.microsoft.com/en-us/library/0tke9fxk.aspx

## 12. Complex scenarios
* Extending logic of Presenter (composition/aggregation is preferable from Unit Testing perspective).
```
var view =new View();
var presenter1 = new Presenter1(view);
var presenter2 = new Presenter2(view, presenter1);
view.Show();
```
* Interaction of several views. 
  * Class diagram:
  
![Sequence Diagram] (https://github.com/agre1981/MVP/blob/master/docs/img/mvp-classdiagram-complex.png)

  * Main view interacts with child within public contract of child view.
```
MainView.SomeMethod()
{
	view1.Refresh();
	view2.Refresh();
}
```
  * Child view interacts with main within itself public events
```
public MainView()
{
	// event subscription
	view1.DataUpdated += this.DataUpdatedHandler;
}

View1.SomeMethod()
{
	// firing event
	DataUpdated();
}
```

## 13. Example of implementation 

checkout https://github.com/agre1981/MVP

  





