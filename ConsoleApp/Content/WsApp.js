(function()
{
 var Global=this,Runtime=this.IntelliFactory.Runtime,EntryPoint;
 Runtime.Define(Global,{
  ConsoleApp:{
   Client:{
    main:function()
    {
     return{
      N:"ne",
      L:"l"
     };
    }
   }
  },
  WebSharper:{
   EntryPoint:{
    Example:Runtime.Field(function()
    {
     return null;
    })
   }
  }
 });
 Runtime.OnInit(function()
 {
  return EntryPoint=Runtime.Safe(Global.WebSharper.EntryPoint);
 });
 Runtime.OnLoad(function()
 {
  EntryPoint.Example();
  return;
 });
}());
