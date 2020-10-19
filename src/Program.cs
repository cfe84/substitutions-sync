using System;
using System.Threading;

namespace Filesync
{
  class Program
  {
    static void Main(string[] args)
    {
      var app = new App("substitutions.json");
      Console.WriteLine("Started");
      Thread.Sleep(-1);
    }
  }
}
