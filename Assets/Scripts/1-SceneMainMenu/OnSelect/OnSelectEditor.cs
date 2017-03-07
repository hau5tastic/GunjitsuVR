using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class OnSelectEditor : GJSelection {
	public override void OnSelect() {
        Screen.fullScreen = false;
        System.Diagnostics.Process.Start("Notepad F:/temp/data.txt");

        /*

         Process foo = new Process();
         foo.StartInfo.FileName = "the_script.bat";
         foo.StartInfo.Arguments = "put your arguments here";
         foo.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
         foo.Start();

        */
    }
}
