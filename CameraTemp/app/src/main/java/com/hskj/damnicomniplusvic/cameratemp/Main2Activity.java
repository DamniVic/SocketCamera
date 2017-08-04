package com.hskj.damnicomniplusvic.cameratemp;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.SharedPreferences;
import android.graphics.Rect;
import android.graphics.YuvImage;
import android.hardware.Camera;
import android.hardware.Camera.Size;
import android.os.Bundle;
import android.preference.PreferenceManager;
import android.view.Menu;
import android.view.MenuItem;
import android.view.SurfaceHolder;
import android.view.SurfaceView;
import android.view.View;
import android.view.View.OnClickListener;
import android.view.WindowManager;
import android.widget.Button;

import java.io.ByteArrayInputStream;
import java.io.ByteArrayOutputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.net.Socket;
import java.net.UnknownHostException;

public class Main2Activity extends Activity implements SurfaceHolder.Callback,
        Camera.PreviewCallback{
    private SurfaceView mSurfaceview = null; // SurfaceView对象：(视图组件)视频显示
    private SurfaceHolder mSurfaceHolder = null; // SurfaceHolder对象：(抽象接口)SurfaceView支持类
    private Camera mCamera = null; // Camera对象，相机预览

    /**服务器地址*/
    private String pUsername="DamniVic";
    /**服务器地址*/
    private String serverUrl="172.16.60.14";
    /**服务器端口*/
    private int serverPort=9635;
    /**视频刷新间隔*/
    private int VideoPreRate=1;
    /**当前视频序号*/
    private int tempPreRate=0;
    /**视频质量*/
    private int VideoQuality=85;

    /**发送视频宽度比例*/
    private float VideoWidthRatio=1;
    /**发送视频高度比例*/
    private float VideoHeightRatio=1;

    /**发送视频宽度*/
    private int VideoWidth=1920;
    /**发送视频高度*/
    private int VideoHeight=1080;
    /**视频格式索引*/
    private int VideoFormatIndex=0;
    /**是否发送视频*/
    private boolean startSendVideo=false;
    /**是否连接主机*/
    private boolean connectedServer=false;

    private Button myBtn01, myBtn02;
    /** Called when the activity is first created. */
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main2);

        //禁止屏幕休眠
        getWindow().setFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON,
                WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);

        mSurfaceview = (SurfaceView) findViewById(R.id.camera_preview);
        myBtn01=(Button)findViewById(R.id.button1);
        myBtn02=(Button)findViewById(R.id.button2);

        //开始连接主机按钮
        myBtn01.setOnClickListener(new OnClickListener(){
            public void onClick(View v) {
                //Common.SetGPSConnected(LoginActivity.this, false);
                if(connectedServer){//停止连接主机，同时断开传输
                    startSendVideo=false;
                    connectedServer=false;
                    myBtn02.setEnabled(false);
                    myBtn01.setText("开始连接");
                    myBtn02.setText("开始传输");
                    //断开连接
                    Thread th = new MySendCommondThread("PHONEDISCONNECT|"+pUsername+"|");
                    th.start();
                }
                else//连接主机
                {
                    //启用线程发送命令PHONECONNECT
                    Thread th = new MySendCommondThread("PHONECONNECT|"+pUsername+"|");
                    th.start();
                    connectedServer=true;
                    myBtn02.setEnabled(true);
                    myBtn01.setText("停止连接");
                }
            }});

        myBtn02.setEnabled(false);
        myBtn02.setOnClickListener(new OnClickListener(){
            public void onClick(View v) {
                if(startSendVideo)//停止传输视频
                {
                    startSendVideo=false;
                    myBtn02.setText("开始传输");
                }
                else{ // 开始传输视频
                    startSendVideo=true;
                    myBtn02.setText("停止传输");
                }
            }});
    }

    @Override
    public void onStart()//重新启动的时候
    {
        mSurfaceHolder = mSurfaceview.getHolder(); // 绑定SurfaceView，取得SurfaceHolder对象
        mSurfaceHolder.addCallback(this); // SurfaceHolder加入回调接口
        mSurfaceHolder.setType(SurfaceHolder.SURFACE_TYPE_PUSH_BUFFERS);// 设置显示器类型，setType必须设置
        //读取配置文件
        SharedPreferences preParas = PreferenceManager.getDefaultSharedPreferences(Main2Activity.this);
        pUsername=preParas.getString("Username", "DamniVic");
        serverUrl=preParas.getString("ServerUrl", "172.16.60.14");
        String tempStr=preParas.getString("ServerPort", "9635");
        serverPort=Integer.parseInt(tempStr);
        tempStr=preParas.getString("VideoPreRate", "1");
        VideoPreRate=Integer.parseInt(tempStr);
        tempStr=preParas.getString("VideoQuality", "85");
        VideoQuality=Integer.parseInt(tempStr);
        tempStr=preParas.getString("VideoWidthRatio", "100");
        VideoWidthRatio=Integer.parseInt(tempStr);
        tempStr=preParas.getString("VideoHeightRatio", "100");
        VideoHeightRatio=Integer.parseInt(tempStr);
        VideoWidthRatio=VideoWidthRatio/100f;
        VideoHeightRatio=VideoHeightRatio/100f;

        super.onStart();
    }

    @Override
    protected void onResume() {
        super.onResume();
        InitCamera();
    }

    /**初始化摄像头*/
    private void InitCamera(){
        try{
            mCamera = Camera.open();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    @Override
    protected void onPause() {
        super.onPause();
        try{
            if (mCamera != null) {
                mCamera.setPreviewCallback(null); // ！！这个必须在前，不然退出出错
                mCamera.stopPreview();
                mCamera.release();
                mCamera = null;
            }
        } catch (Exception e) {
            e.printStackTrace();
        }
    }
    @Override
    public void surfaceChanged(SurfaceHolder arg0, int arg1, int arg2, int arg3) {
        // TODO Auto-generated method stub
        if (mCamera == null) {
            return;
        }
        mCamera.stopPreview();
        mCamera.setPreviewCallback(this);
        mCamera.setDisplayOrientation(90); //设置横行录制
        //获取摄像头参数
        Camera.Parameters parameters = mCamera.getParameters();
        Size size = parameters.getPreviewSize();
        VideoWidth=size.width;
        VideoHeight=size.height;
        VideoFormatIndex=parameters.getPreviewFormat();

        mCamera.startPreview();
    }

    @Override
    public void surfaceCreated(SurfaceHolder holder) {
        // TODO Auto-generated method stub
        try {
            if (mCamera != null) {
                mCamera.setPreviewDisplay(mSurfaceHolder);
                mCamera.startPreview();
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    @Override
    public void surfaceDestroyed(SurfaceHolder holder) {
        // TODO Auto-generated method stub
        if (null != mCamera) {
            mCamera.setPreviewCallback(null); // ！！这个必须在前，不然退出出错
            mCamera.stopPreview();
            mCamera.release();
            mCamera = null;
        }
    }

    @Override
    public void onPreviewFrame(byte[] data, Camera camera) {
        // TODO Auto-generated method stub
        //如果没有指令传输视频，就先不传
        if(!startSendVideo)
            return;
        if(tempPreRate<VideoPreRate){
            tempPreRate++;
            return;
        }
        tempPreRate=0;
        try {
            if(data!=null)
            {
                YuvImage image = new YuvImage(data,VideoFormatIndex, VideoWidth, VideoHeight,null);
                if(image!=null)
                {
                    ByteArrayOutputStream outstream = new ByteArrayOutputStream();
                    //在此设置图片的尺寸和质量
                    image.compressToJpeg(new Rect(0, 0, (int)(VideoWidthRatio*VideoWidth),
                            (int)(VideoHeightRatio*VideoHeight)), VideoQuality, outstream);
                    outstream.flush();
                    //启用线程将图像数据发送出去
                    Thread th = new MySendFileThread(outstream,pUsername,serverUrl,serverPort);
                    th.start();
                }
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    //将YUVImage旋转90度
    private byte[] rotateYUV420Degree90(byte[] data, int imageWidth, int imageHeight)
    {
        byte [] yuv = new byte[imageWidth*imageHeight*3/2];
        // Rotate the Y luma
        int i = 0;
        for(int x = 0;x < imageWidth;x++)
        {
            for(int y = imageHeight-1;y >= 0;y--)
            {
                yuv[i] = data[y*imageWidth+x];
                i++;
            }
        }
        // Rotate the U and V color components
        i = imageWidth*imageHeight*3/2-1;
        for(int x = imageWidth-1;x > 0;x=x-2)
        {
            for(int y = 0;y < imageHeight/2;y++)
            {
                yuv[i] = data[(imageWidth*imageHeight)+(y*imageWidth)+x];
                i--;
                yuv[i] = data[(imageWidth*imageHeight)+(y*imageWidth)+(x-1)];
                i--;
            }
        }
        return yuv;
    }

    /**创建菜单*/
    public boolean onCreateOptionsMenu(Menu menu)
    {
        menu.add(0,0,0,"系统设置");
        menu.add(0,1,1,"关于程序");
        menu.add(0,2,2,"退出程序");
        return super.onCreateOptionsMenu(menu);
    }
    /**菜单选中时发生的相应事件*/
    public boolean onOptionsItemSelected(MenuItem item)
    {
        super.onOptionsItemSelected(item);//获取菜单
        switch(item.getItemId())//菜单序号
        {
            case 0:
                //系统设置
            {
//                Intent intent=new Intent(this,SettingActivity.class);
//                startActivity(intent);
            }
            break;
            case 1://关于程序
            {
                new AlertDialog.Builder(this)
                        .setTitle("关于本程序")
                        .setMessage("本程序由武汉大学水利水电学院肖泽云设计、编写。\nEmail：xwebsite@163.com")
                        .setPositiveButton
                                (
                                        "我知道了",
                                        new DialogInterface.OnClickListener()
                                        {
                                            @Override
                                            public void onClick(DialogInterface dialog, int which)
                                            {
                                            }
                                        }
                                )
                        .show();
            }
            break;
            case 2://退出程序
            {
                //杀掉线程强制退出
                android.os.Process.killProcess(android.os.Process.myPid());
            }
            break;
        }
        return true;
    }

    /**发送命令线程*/
    class MySendCommondThread extends Thread{
        private String commond;
        public MySendCommondThread(String commond){
            this.commond=commond;
        }
        public void run(){
            //实例化Socket
            try {
                Socket socket=new Socket(serverUrl,serverPort);
                PrintWriter out = new PrintWriter(socket.getOutputStream());
                out.println(commond);
                out.flush();
            } catch (UnknownHostException e) {
            } catch (IOException e) {
            }
        }
    }

    /**发送文件线程*/
    class MySendFileThread extends Thread{
        private String username;
        private String ipname;
        private int port;
        private byte byteBuffer[] = new byte[1024];
        private OutputStream outsocket;
        private ByteArrayOutputStream myoutputstream;

        public MySendFileThread(ByteArrayOutputStream myoutputstream,String username,String ipname,int port){
            this.myoutputstream = myoutputstream;
            this.username=username;
            this.ipname = ipname;
            this.port=port;
            try {
                myoutputstream.close();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }

        public void run() {
            try{
                //将图像数据通过Socket发送出去
                Socket tempSocket = new Socket(ipname, port);
                outsocket = tempSocket.getOutputStream();
                //写入头部数据信息
                String msg=java.net.URLEncoder.encode("PHONEVIDEO|"+username+"|","utf-8");
                byte[] buffer= msg.getBytes();
                outsocket.write(buffer);

                ByteArrayInputStream inputstream = new ByteArrayInputStream(myoutputstream.toByteArray());
                int amount;
                while ((amount = inputstream.read(byteBuffer)) != -1) {
                    outsocket.write(byteBuffer, 0, amount);
                }
                myoutputstream.flush();
                myoutputstream.close();
                tempSocket.close();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
    }
}