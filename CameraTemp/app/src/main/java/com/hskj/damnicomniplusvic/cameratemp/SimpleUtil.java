package com.hskj.damnicomniplusvic.cameratemp;

import android.app.ActivityManager;
import android.app.ActivityManager.RunningTaskInfo;
import android.content.ComponentName;
import android.content.Context;
import android.graphics.Bitmap;
import android.graphics.Point;
import android.os.Environment;
import android.view.View;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.util.List;

/**
 * Created by DAMNICOMNIPLUSVIC on 2017/5/3.
 * (c) 2017 DAMNICOMNIPLUSVIC Inc,All Rights Reserved.
 */

public class SimpleUtil {
    //检查当前点是否超出view的范围
    public static boolean checkOutOfView(float x, float y, View view) {
        float left = view.getX();
        float top = view.getY();
        x += left;
        y += top;
        float right = left + view.getWidth();
        float bottom = top + view.getHeight();
        if (x > right || x < left)
            return false;
        if (y < top || y > bottom)
            return false;
        return true;
    }


    //计算当前image在imageview中的原点坐标,因为image的宽高单位和imageview的宽高单位不一致，所以需要计算
    public static Point[] calcOriginPoint(View view, Bitmap bitmap) {
        Point[] points = new Point[2];
        Point point = new Point();
        Point point1 = new Point();
        float viewWidth = view.getWidth();
        float viewHeight = view.getHeight();
        float bitmapWidth = bitmap.getWidth();
        float bitmapHeight = bitmap.getHeight();
        if (viewWidth / bitmapWidth > viewHeight / bitmapHeight) {
            point.x = (int) ((viewWidth - viewHeight / bitmapHeight * bitmapWidth) / 2);
            point.y = 0;
            points[0] = point;
            point1.x = (int) (viewWidth - 2 * point.x);
            point1.y = (int) viewHeight;
            points[1] = point1;
        } else {
            point.x = 0;
            point.y = (int) ((viewHeight - viewWidth / bitmapWidth * bitmapHeight) / 2);
            points[0] = point;
            point1.x = (int) viewWidth;
            point1.y = (int) (viewHeight - point.y * 2);
            points[1] = point1;
        }
        return points;
    }

    //保存图片到手机存储中
    public static String saveBitmap(Bitmap bitmap) {
        File file = null;
        FileOutputStream foutput = null;
        try {
            String dir = Environment.getExternalStoragePublicDirectory("").getAbsolutePath() + "/CtsVideo/CutImage";
            File dirFile = new File(dir);
            if (!dirFile.exists())
                dirFile.mkdirs();
            String logoPath = dir + "/" + System.currentTimeMillis() + ".png";
            File logoFile = new File(logoPath);
            if (!logoFile.exists())
                try {
                    logoFile.createNewFile();
                } catch (IOException e) {
                    e.printStackTrace();
                }
            foutput = new FileOutputStream(logoFile);
            bitmap.compress(Bitmap.CompressFormat.PNG, 100, foutput);
            file = logoFile;
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            try {
                foutput.close();
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
        if (file != null)
            return file.getAbsolutePath();
        else
            return null;
    }

    //创建一个新的文件用来接收合成后的视频
    public static String createFile() {
        String dir= Environment.getExternalStoragePublicDirectory("").getAbsolutePath()+"/CtsVideo/Video";
        File dirFile=new File(dir);
        if(!dirFile.exists())
            dirFile.mkdirs();
        String logoPath=dir+"/"+ System.currentTimeMillis()+".mp4";
        File logoFile=new File(logoPath);
        if(!logoFile.exists())
            try {
                logoFile.createNewFile();
            } catch (IOException e) {
                e.printStackTrace();
            }
        if (logoFile != null)
            return logoFile.getAbsolutePath();
        else
            return null;
    }

    //判断当前应用是否处于前台
    public static boolean isBackground(Context context) {
        ActivityManager am = (ActivityManager) context.getSystemService(Context.ACTIVITY_SERVICE);
        List<RunningTaskInfo> tasks = am.getRunningTasks(1);
        if (!tasks.isEmpty()) {
            ComponentName topActivity = tasks.get(0).topActivity;
            if (!topActivity.getPackageName().equals(context.getPackageName())) {
                return true;
            }
        }
        return false;
    }
}
