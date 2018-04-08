using UnityEngine;
using System.IO;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class VeuiEditor : EditorWindow {
	public static VeuiEditor mInstance = null;
	// Use this for initialization
	public static int lena=10;
    public static int zpx = 25;
    public static int zpy = 35;
    public static int mww;
    public static int mwh;
    public string curkey="";
    public int lenb = 5;
	public int tiw=200;
	public int tbh = 30;
	public int iconh = 22;
    public int iconha = 28;
    public int iconw = 25;
    public int iconwa = 38;
    public int namewidth = 50;
    public static int sel = 0;
    public static string path = "";
    public static Dictionary<string , Rect> map;
    //public static Dictionary<string, Rect> _map;
    public static Dictionary<string, Ctrbox> ctrlist;
    //public static Dictionary<string, Ctrbox> _ctrlist;
    public Vector2 stpos = Vector2.zero;
    //public static Dictionary <>
    public static Rollout aroll;
    public static Scalehandle ash;
    public static VUEPanel apan;
    public static List<Ctrbox> pctr;
    //todo: try move roll panel into ctrlist
    public static bool snapon=false;
    public static float gridstep = 8;

    public static float postdit(float p,float d)
    {
        float r;
        if (snapon)
        {
            r = Mathf.FloorToInt((p - d) / gridstep) * gridstep;
        }
        else
        {
            r = p-d;
        }
       
        return r;
    }
    public static void setsel(Ctrbox ctr)
    {
        pctr = new List<Ctrbox>();
        pctr.Add(ctr);
        ash.setsel(ctr);
    }
    public static bool unregmap(string gid)
    {
        if (!map.ContainsKey(gid))
        { return false; }
        else
        {
            map.Remove (gid);
            ctrlist.Remove (gid);
            //foreach (var ctr in map) Debug.Log(ctr);
            return true;
        }

    }
    public static string regmap(Rect wr,Ctrbox cbox)
    {
        string gid= System.Guid.NewGuid().ToString ();
        if (map.ContainsKey(gid))
        { return ""; }
        else
        {
            map.Add(gid, wr);
            ctrlist.Add(gid, cbox);
            //foreach (var ctr in map) Debug.Log(ctr);
            return gid;
         }
    }
    public static bool refreshmap(string gid, Rect  wr)
    {
        if (!map.ContainsKey(gid))
        { return false; }
        else
        {
            map[gid] = wr;
            //ctrlist.Remove(gid);
            //foreach (var ctr in map) Debug.Log(ctr);
            return true;
        }
    }
    public static void updatesh()
    {
        ash.rrmap();
        mInstance.Repaint();
    }
    public void Awake()
    {
        var script = MonoScript.FromScriptableObject(this);
        path = AssetDatabase.GetAssetPath(script);
        path = Path.GetDirectoryName(path);
        path += "/res/icon/";

        //Debug.Log(path);
        
        map = new Dictionary<string, Rect>();
        ctrlist = new Dictionary<string,Ctrbox>();
        pctr = new List<Ctrbox>();
        //pctr.Add(ctr);

        //aroll.wr = new Rect(zpx, zpy, 500, 350);
    }
	[MenuItem ("Scene/Visual EditorUI Layout", false, 2)]
	public static void Veuilayout ()
	{
		
		if (mInstance == null) {
			mInstance = (VeuiEditor)EditorWindow.GetWindow (typeof(VeuiEditor));
			mInstance.titleContent.text = "Visual UI Layout";
			mInstance.minSize = new Vector2 (700, 500);
			mInstance.maxSize = new Vector2 (5200, 5000);
			mInstance.Show ();
            Rect tmpwr = new Rect(zpx, zpy, 500, 350);
            aroll = new Rollout(tmpwr);
            //aroll.gid = "mainroll";
            //aroll.gid=regmap(tmpwr, aroll);
            pctr.Add(aroll);
            ash = new Scalehandle(tmpwr);
            apan = new VUEPanel(tmpwr);
            
        }
        foreach (var ctr in map) Debug.Log(ctr);
        //VeuiEditor.showpth();
    }
	public void OnGUI()
	{
        //Debug.Log(path);
        Texture2D [] tlist=new Texture2D[2];
		tlist [0] = new Texture2D(1,1);
		tlist[1] = new Texture2D(1,1);
        GUIStyle guiStyle = new GUIStyle();
        guiStyle.padding = new RectOffset(0, 0, 0, 0);
                
        guiStyle.active.background= (Texture2D) AssetDatabase.LoadAssetAtPath(path + "icon_d.png", typeof(Texture2D));
        //guiStyle.active.background.LoadImage(File.ReadAllBytes("./Assets/VisualEditorUILayout/Editor/res/icon/icon_d.png"));
        guiStyle.onNormal.background = guiStyle.active.background;
        tlist[0]= (Texture2D)AssetDatabase.LoadAssetAtPath(path + "icon_0.png", typeof(Texture2D));
        tlist[1] = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "icon_1.png", typeof(Texture2D));
        //tlist[0].LoadImage(File.ReadAllBytes("./Assets/VisualEditorUILayout/Editor/res/icon/icon_0.png"));
		//tlist[1].LoadImage(File.ReadAllBytes("./Assets/VisualEditorUILayout/Editor/res/icon/icon_1.png"));
		Rect wr = new Rect (200, 200, 700, 500);

		if (mInstance != null) {
			wr = mInstance.position;
			//Debug.Log ("here");

		}
		int ww = (int)wr.width;
		int wh = (int)wr.height;
        Event cure = Event.current;
        if (cure != null&&(cure.type==EventType.MouseDown|| cure.type == EventType.MouseDrag|| cure.type == EventType.MouseUp))
        {

            //Debug.Log(cure);
            var mpos = cure.mousePosition;
            if (cure.type == EventType.MouseDown)
            {
                curkey = "";
            }
            if (curkey == "")
            {
                foreach (string key in map.Keys)
                {
                    if (map[key].Contains(mpos))
                    {

                        curkey = key;
                        Debug.Log(map[key]);
                        break;
                    }

                }
                if (curkey == "")
                {
                    if (aroll.wr.Contains(mpos))
                    {
                        aroll.reciver(cure, "");
                    }
                }
            }
            if (curkey != "")
            {
                ctrlist[curkey].reciver(cure, curkey);
                //Debug.Log(map[curkey]);
                //Debug.Log("kkkk");
            }
            if(cure.type == EventType.MouseUp)
            {
                curkey = "";
            }
            
        }
        wr.width-=2*lena+tiw+lenb;
		wr.height -= 2*lena+tbh+lenb;
        mww = (int)wr.width;
        mwh = (int)wr.height;
        //		if (vlen > wh)
        //			vlen = wh;

        
        wr.x = lena;
		wr.y = lena;
        //wr.height =vlen- 20;
        //wr.width = wr.height;
        
        stpos=GUI.BeginScrollView(wr, stpos,new Rect(lena,lena,2000,1000),true,true);

        //GUI.Box(wr, "--------");

        if (aroll != null)
        {
            aroll.paint();
            Vector2 sp = Vector2.zero;
            sp = GUI.BeginScrollView(aroll.wr, sp, aroll.wr);
            if (ctrlist != null) foreach (var ctr in ctrlist)
                {
                    ctr.Value.paint();
                }

            GUI.EndScrollView();
        }
        if (ash != null) ash.paint();
        GUI.EndScrollView();
		wr.x += wr.width + lenb;
		wr.width = tiw ;
		GUI.Box(wr, "Property");
        if (pctr != null) 
            {
                pctr[0].paintprop(wr);
            }
        wr.x = lena;
		wr.y += wr.height+lenb;
		wr.width = ww - 2 * lena-tiw-lenb;
		wr.height = tbh;
		GUI.Box(wr,"");

		wr.y += (tbh - iconh) / 2;
		wr.width = iconw*2;
		wr.height = iconh;


        sel = GUI.Toolbar(wr, sel, tlist, guiStyle);

        wr.x = ww - lena - iconwa;
        wr.y = wh - lena-iconha;
        wr.width = iconwa;
        wr.height = iconha;
        if(GUI.Button(wr, "s"))
        {
            OutStrings oss = new OutStrings();
            oss.OutInc();
            oss.BeginCls("test");
            oss.OutMenu();
            oss.BeginGui();
            foreach (var ctr in VeuiEditor.ctrlist)
            {
                oss.OutCtr(ctr.Value);
            }
            oss.EndGui();
            oss.EndCls();
            Debug.Log(oss.output);
        }

        wr.x -= iconwa;
        GUI.Button(wr, "o");

        wr.x -= iconwa;
        GUI.Button(wr, "n");

        wr.x -= 2*iconwa;
        wr.width += 5;
        snapon=GUI.Toggle(wr, snapon, "Grid");


    }
}




public class Ctrbox
{
    public string type;
    public string gid;
    public Rect wr;
    public Texture2D bimg;
    public bool sld = false;
    public float dx = 0;
    public float dy = 0;
    public string name = "";
    public string caption = "";

   // public Scalehandle sh;
    public virtual void reciver(Event cur, string gid)
    {
        if (VeuiEditor.sel == 0 && cur.type == EventType.MouseDown)
        {
            VeuiEditor.setsel(this);
            dx = cur.mousePosition.x - wr.x;
            dy = cur.mousePosition.y - wr.y;

            //Debug.Log("inbutton");
        }
        if (VeuiEditor.sel == 0 && cur.type == EventType.MouseDrag)
        {

            wr.x = VeuiEditor.postdit(cur.mousePosition.x, dx);
            wr.y = VeuiEditor.postdit(cur.mousePosition.y, dy);

            rrmap();
            VeuiEditor.updatesh();
        }
        VeuiEditor.mInstance.Repaint();
    }
    public virtual void paintprop(Rect awr)

    {
        var pwr = awr;
        int iconh = 28;
        int namewidth = 60;
        pwr.height = iconh;
        pwr.y += iconh;
        GUI.Label(pwr, "name");
        pwr.y += iconh;
        GUI.Label(pwr, "caption");
        pwr = awr;
        pwr.y += iconh;
        pwr.height = iconh;
        pwr.x += namewidth;
        pwr.width -= namewidth;
        name=GUI.TextField(pwr, name);
        pwr.y += iconh;
        caption=GUI.TextField(pwr, caption);
    }
    public virtual void rrmap() { }
    public virtual void paint() { }
    public virtual  void updatewr(Rect tmpwr)
    {
        wr = tmpwr;
    }
    public virtual void urmap() { }
    public virtual void rgmap(Rect tmpr) { }
}

public class VUEPanel : Ctrbox
{
    public VUEPanel(Rect tmpwr)
    {
        wr = tmpwr;
        //gid = VeuiEditor.regmap(wr, this);
    }
    public override void reciver(Event cur, string gid)
    {
        
    }
    public override void rgmap(Rect tmpr)
    {
        
    }
    public override void rrmap()
    {
        
    }
}
public class VUEButton :Ctrbox
{
    private float dw = 50;
    private float dh = 18;
    
    public override void rrmap()
    {
        VeuiEditor.refreshmap(gid, wr);
    }
    public override void reciver(Event cur, string gid)
    {
        base.reciver(cur, gid);
    }
    public VUEButton (Vector2 mpos)
    {
        type = "Button";
        name = "Btn";
        Rect wrtmp = new Rect(mpos.x, mpos.y, dw, dh);
        wr = wrtmp;
        gid = VeuiEditor.regmap(wrtmp, this);
        //sh = new Scalehandle(this, wr);
       
    }
    public override void paint()
    {
        GUI.Button(wr,caption);
    }
    
}

public class Rollout:Ctrbox
{

    
    public  Rollout(Rect tmpwr)
    {
        Debug.Log("here");
        //Debug.Log(wr);
        //Debug.Log("-----------");
        //sld = true;
        wr = tmpwr;
        //sh = new Scalehandle(this,wr);
        //sh.pctr = this;
        

        //gid=VeuiEditor.regmap(wr, this);
    }
    public override void updatewr(Rect tmpwr)
    {
        base.updatewr(tmpwr);
    }
    public override  void rrmap()
    {
       // VeuiEditor.refreshmap(gid, wr);
    }
    
    public override void paint()
    {
        //Vector2 spos = GUI.BeginScrollView(wr, spos, wr);
        GUI.Box(wr, "" );
        //GUI.EndScrollView();
        if (sld==false) 
        {
            
        }else{
            //sh.paint();
        }
    }
    public override void reciver(Event cure, string gid)
    {
        base.reciver(cure, gid);

        if (VeuiEditor.sel==1&&cure.type ==EventType.MouseDown )
        {
            var tb = new VUEButton(cure.mousePosition);
            //VeuiEditor.ctrlist.Add(tb.gid, tb);
            
        }
        VeuiEditor.mInstance.Repaint();

    }
}

public class Scalehandle:Ctrbox 
{
    public int lena = 6;
    public int lenb = 35;
    public bool bepaint = true;
    public string gidlt;
    public string gidlm;
    public string gidlb;
    public string gidtm;
    public string gidbm;
    public string gidrt;
    public string gidrm;
    public string gidrb;
    public int minx;
    public int miny;
    public int maxx;
    public int maxy;
    //public int state = 0;
   
    public Rect calrect()
    {
        wr = VeuiEditor.pctr[0].wr;
        return (VeuiEditor.pctr[0].wr);
        
    }
    public void setsel(Ctrbox ctr)
    {
        if (ctr!=null)
        {
            bepaint = true;
            urmap();
            Rect tmprect = calrect();
            rgmap(tmprect);
        }
    }

    public override void reciver(Event cur,string gid)
    {

        Debug.Log(cur.type);
        //if(gid==gidlt )
        
        if (cur.type == EventType.MouseDrag)
        {
            Vector2 mpos = new Vector2();
            mpos=cur.mousePosition;
            mpos.x = VeuiEditor.postdit(mpos.x, 0);
            mpos.y = VeuiEditor.postdit(mpos.y, 0);
            Vector2 v2a = new Vector2(wr.x,wr.y);
            Vector2 v2b = new Vector2(wr.x + wr.width, wr.y + wr.height);
            //Debug.Log(v2b);
            //Debug.Log(wr);
            //Debug.Log("00000000000000");
            Rect tmpwr = wr;
            if (gid==gidlt)
            {
                v2a.x = (mpos.x<minx)?minx:mpos.x;
                v2a.x = (v2a.x < v2b.x - 3*lena) ? v2a.x : v2b.x - 3*lena;

                v2a.y = (mpos.y < miny) ? miny : mpos.y;
                v2a.y = (v2a.y < v2b.y - 3 * lena) ? v2a.y : v2b.y - 3 * lena;
                //v2b = new Vector2(minx + wr.width,miny+wr.height);
                
            }
            if (gid == gidlm)
            {
                v2a.x = (mpos.x < minx) ? minx : mpos.x;
                v2a.x = (v2a.x < v2b.x - 3 * lena) ? v2a.x : v2b.x - 3 * lena;
            }
            if (gid == gidlb)
            {
                v2a.x = (mpos.x < minx) ? minx : mpos.x;
                v2a.x = (v2a.x < v2b.x - 3 * lena) ? v2a.x : v2b.x - 3 * lena;

                v2b.y = (mpos.y < miny) ? miny : mpos.y;
                v2b.y = (v2b.y > v2a.y + 3 * lena) ? v2b.y : v2a.y + 3 * lena;
            }
            if (gid == gidtm)
            {
                v2a.y = (mpos.y < miny) ? miny : mpos.y;
                v2a.y = (v2a.y < v2b.y - 3 * lena) ? v2a.y : v2b.y - 3 * lena;
            }
            if (gid == gidbm)
            {
                v2b.y = (mpos.y < miny) ? miny : mpos.y;
                v2b.y = (v2b.y > v2a.y + 3 * lena) ? v2b.y : v2a.y + 3 * lena;
            }
            if (gid == gidrt)
            {
                v2b.x = (mpos.x < minx) ? minx : mpos.x;
                v2b.x = (v2b.x > v2a.x + 3 * lena) ? v2b.x : v2a.x + 3 * lena;

                v2a.y = (mpos.y < miny) ? miny : mpos.y;
                v2a.y = (v2a.y < v2b.y - 3 * lena) ? v2a.y : v2b.y - 3 * lena;
            }
            if (gid == gidrm)
            {
                v2b.x = (mpos.x < minx) ? minx : mpos.x;
                v2b.x = (v2b.x > v2a.x + 3 * lena) ? v2b.x : v2a.x + 3 * lena;
            }
            if (gid == gidrb)
            {
                v2b.x = (mpos.x < minx) ? minx : mpos.x;
                v2b.x = (v2b.x > v2a.x + 3 * lena) ? v2b.x : v2a.x + 3 * lena;
                v2b.y = (mpos.y < miny) ? miny : mpos.y;
                v2b.y = (v2b.y > v2a.y + 3 * lena) ? v2b.y : v2a.y + 3 * lena;
            }
                tmpwr.x = v2a.x;
            tmpwr.y = v2a.y;

            tmpwr.width = v2b.x - v2a.x;
            tmpwr.height = v2b.y - v2a.y;
            
            wr = tmpwr;
            foreach(var ctr in VeuiEditor.pctr)
            {
                ctr.updatewr(wr);
            }
            rrmap();
            foreach (var ctr in VeuiEditor.pctr)
            {
                ctr.rrmap();
               // Debug.Log("kkllsj");
            }
            VeuiEditor.mInstance.Repaint();
        }
        else if (cur.type == EventType.MouseUp ||cur.type==EventType.MouseDown)
        {
            // state = 0;
            rrmap();
            foreach(var ctr in VeuiEditor.pctr)
            {
                ctr.rrmap();
                Debug.Log("kkllsj");
            }
            
        }
       
        
    }
    public override  void rrmap()
    {
        calrect();
        Rect wrtmp = new Rect(wr.x - lena, wr.y - lena, lena, lena);
        VeuiEditor.refreshmap(gidlt, wrtmp);
        Debug.Log(wr);
        wrtmp.y = wr.y + wr.height;
        VeuiEditor.refreshmap(gidlb,wrtmp);

        wrtmp.x = wr.width + wr.x;
        //gidrb = VeuiEditor.regmap(wrtmp, this);
        VeuiEditor.refreshmap(gidrb, wrtmp);

        wrtmp.y = wr.y - lena;
        //gidrt = VeuiEditor.regmap(wrtmp, this);
        VeuiEditor.refreshmap(gidrt, wrtmp);

        wrtmp = new Rect(wr.x - lena, wr.y - lena, lena, lena);
        wrtmp.y = wr.y + wr.height / 2 - lena / 2;
        //gidlm = VeuiEditor.regmap(wrtmp, this);
        VeuiEditor.refreshmap(gidlm, wrtmp);

        wrtmp.x = wr.width + wr.x;
        //gidrm = VeuiEditor.regmap(wrtmp, this);
        VeuiEditor.refreshmap(gidrm, wrtmp);

        wrtmp.y = wr.y - lena;
        wrtmp.x = wr.width / 2 + wr.x - lena / 2;
        //gidtm = VeuiEditor.regmap(wrtmp, this);
        VeuiEditor.refreshmap(gidtm, wrtmp);

        wrtmp.y = wr.y + wr.height;
        //gidbm = VeuiEditor.regmap(wrtmp, this);
        VeuiEditor.refreshmap(gidbm, wrtmp);
    }
    public  Scalehandle(Rect tmpr)
    {
        //Debug.Log(VeuiEditor.mInstance);
        type = "sh";
        string path = VeuiEditor.path;
        bimg = (Texture2D)AssetDatabase.LoadAssetAtPath(path + "handle_0.png", typeof(Texture2D));
        minx = VeuiEditor.zpx + lena;
        miny = VeuiEditor.zpy + lena;
        maxx = VeuiEditor.mww - VeuiEditor.lena;
        maxy = VeuiEditor.mwh - VeuiEditor.lena;
        rgmap(tmpr);
        
    }
    
    public override void rgmap(Rect tmpr)
    {
        wr = tmpr;
        Rect wrtmp = new Rect(wr.x - lena, wr.y - lena, lena, lena);
        gidlt = VeuiEditor.regmap(wrtmp, this);
       
        wrtmp.y = wr.y + wr.height;
        gidlb = VeuiEditor.regmap(wrtmp, this);

        wrtmp.x = wr.width + wr.x;
        gidrb = VeuiEditor.regmap(wrtmp, this);

        wrtmp.y = wr.y - lena;
        gidrt = VeuiEditor.regmap(wrtmp, this);

        wrtmp = new Rect(wr.x - lena, wr.y - lena, lena, lena);
        wrtmp.y = wr.y + wr.height / 2 - lena / 2;
        gidlm = VeuiEditor.regmap(wrtmp, this);

        wrtmp.x = wr.width + wr.x;
        gidrm = VeuiEditor.regmap(wrtmp, this);

        wrtmp.y = wr.y - lena;
        wrtmp.x = wr.width / 2 + wr.x - lena / 2;
        gidtm = VeuiEditor.regmap(wrtmp, this);

        wrtmp.y = wr.y + wr.height;
        gidbm = VeuiEditor.regmap(wrtmp, this);
    }
    public override void urmap()
    {
        //Debug.Log("here");
        VeuiEditor.unregmap(gidlt);
        VeuiEditor.unregmap(gidlb);
        VeuiEditor.unregmap(gidrb);
        VeuiEditor.unregmap(gidrt);
        VeuiEditor.unregmap(gidlm);
        VeuiEditor.unregmap(gidrm);
        VeuiEditor.unregmap(gidtm);
        VeuiEditor.unregmap(gidbm);
        //Debug.Log("outhere");
        //Debug.Log(pctr);
    }
    public override  void paint()
    {
        if (bepaint)
        {
            GUIStyle guiStyle = new GUIStyle();
            guiStyle.padding = new RectOffset(0, 0, 0, 0);
            Rect wrtmp = new Rect(wr.x - lena, wr.y - lena, lena, lena);
            GUI.Box(wrtmp, bimg, guiStyle);

            wrtmp.y = wr.y + wr.height;
            GUI.Box(wrtmp, bimg, guiStyle);

            wrtmp.x = wr.width + wr.x;
            GUI.Box(wrtmp, bimg, guiStyle);

            wrtmp.y = wr.y - lena;
            GUI.Box(wrtmp, bimg, guiStyle);

            wrtmp = new Rect(wr.x - lena, wr.y - lena, lena, lena);

            wrtmp.y = wr.y + wr.height / 2 - lena / 2;
            GUI.Box(wrtmp, bimg, guiStyle);

            wrtmp.x = wr.width + wr.x;
            GUI.Box(wrtmp, bimg, guiStyle);

            wrtmp.y = wr.y - lena;
            wrtmp.x = wr.width / 2 + wr.x - lena / 2;
            GUI.Box(wrtmp, bimg, guiStyle);

            wrtmp.y = wr.y + wr.height;
            GUI.Box(wrtmp, bimg, guiStyle);

            //Debug.Log(wr);
        }
    }
}
public class OutStrings
{
    public string output = "";
    public string Rect2Str(Rect wr)
    {
        string swr= "new Rect(";
        swr += wr.x+",";
        swr += wr.y + ",";
        swr += wr.width + ",";
        swr += wr.height + ")";
        return swr;
    }
    public void OutInc()
    {
        string o = "using UnityEngine;\n";
        o += "using UnityEditor;\n";
        o += "using System.Collections;\n";
        output += o;
    }
    public void OutMenu()
    {
        string o= "[MenuItem(\"test/test\", false, 2)]:";
        o += "public static void testwd()\n";
        o += "{\n";
        o += "EditorWindow.GetWindow(typeof(test));\n";
        o += "}\n";
        output+=o;
    }

    public void BeginCls(string cname)
    {

        output+= ("public class " + cname + "\n{\n");
    }
    public void BeginGui()
    {
        output+= ("public void OnGUI()\n{\n");
    
    }
    public void EndGui()
    {
        output+= ("}\n");
    }
    public void EndCls()
    {
        output+= "}\n";
    }

    public void OutCtr(Ctrbox ctr)
    {
        if(ctr.type!="sh")
        output+= ("GUI."+ctr.type+"("+ Rect2Str(ctr.wr) + ",\""+ctr.name+"\");\n");
    }
}