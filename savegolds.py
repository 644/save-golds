
#!/usr/bin/env python
# title             : savegolds.py
# description       : Saves replay buffer of your best livesplit segment
# authors           : hies, Bootscreen
# date              : 2020 08 06
# version           : 1.0-0
# dependencies      : Python 3.6, livesplit, livesplit.server, moviepy
# notes             : Setup guide:
#                   : Livesplit:
#                   :   1. Install livesplit server https://github.com/LiveSplit/LiveSplit.Server#install
#                   :      Make sure to add it to your layout and right click > control > start server
#                   :
#                   : Python:
#                   :   1. Install python (v3.6 64bit, this is important)
#                   :      direct https://www.python.org/ftp/python/3.6.8/python-3.6.8-amd64.exe
#                   :      or https://www.python.org/downloads/release/python-368/ and find the "Windows x86-64 executable installer"
#                   :
#                   :      Make sure to select "add to path" when installing
#                   :
#                   :   2. Install moviepy
#                   :      Open cmd and type pip install moviepy
#                   :
# python_version    : 3.6 (Must not be newer)
# ==============================================================================

#import obspython as obs
import os, sys, subprocess, time, socket, glob, re, select
from time import sleep, perf_counter
from shutil import copyfile 
from importlib import util 
from datetime import datetime
from moviepy.config import get_setting
from moviepy.tools import subprocess_call
from moviepy.editor import VideoFileClip
from moviepy.config import get_setting
from moviepy.tools import subprocess_call

# location for recordings
dir = "D:/replays/"
bkpdir = "D:/replays/old/"
recfolder = "D:/replays/recordings/"

# livesplit.server methods
getindex = str("getsplitindex\r\n").encode()
getsplitname = str("getprevioussplitname\r\n").encode()
getsplittime = str("getlastsplittime\r\n").encode()
getcurtime = str("getcurrenttime\r\n").encode()
getcomp = str("getcomparisonsplittime\r\n").encode()
getstatus = str("getcurrenttimerphase\r\n").encode()

# defaults
previndex = -1
starttime = 0
enabled = True
debug_mode = True
lasttime = None
oldpath = None
outfile = None
totaltime = None
recs = None

def file_in_use(fpath):
    if os.path.exists(fpath):
        try:
            os.rename(fpath, fpath)
            return False
        except:
            return True
    
def ffmpeg_extract_subclip(filename, t1, t2, targetname=None):
    sleep(1)
    name, ext = os.path.splitext(filename)
    if not targetname:
        T1, T2 = [int(1000*t) for t in [t1, t2]]
        targetname = "%sSUB%d_%d.%s" % (name, T1, T2, ext)
    
    cmd = [get_setting("FFMPEG_BINARY"),"-y",
            "-ss", "%0.2f"%t1,
            "-i", filename,
            "-t", "%0.2f"%(t2-t1),
            "-vcodec", "copy", "-acodec", "copy", targetname]
    
    if debug_mode:
        subprocess_call(cmd)
    else:
        subprocess_call(cmd, None)
    
def cut_replay(seconds, new_path, outfile, remove, from_end, last_replay):
    global debug_mode
    if debug_mode:
        print("[AR] save_replay")
        print("[AR] seconds=" + str(seconds) )
        print("[AR] path=" + new_path)
        print("[AR] remove=%s" %(remove))
        print("[AR] from_end=%s" %(from_end))
        
    if not enabled:
        return
    
    if seconds > 0:
        if last_replay is not None and len(last_replay) > 0:
            if debug_mode: 
                print("[AR] last_replay=" + last_replay)
                
            last_replay_folder = os.path.dirname(os.path.abspath(last_replay))
            last_replay_name, last_replay_type = os.path.splitext(os.path.basename(last_replay))
            
            if len(new_path) <= 0 or not os.path.exists(new_path):
                new_path = last_replay_folder
            
            if debug_mode: 
                print("[AR] last_replay_folder=" + last_replay_folder)
                print("[AR] last_replay_name=" + last_replay_name)
                print("[AR] last_replay_type=" + last_replay_type)
                print("[AR] new_path=" + new_path)

            new_replay = os.path.join(new_path, outfile)
               
            if debug_mode: 
                print("[AR] last_replay=" + last_replay)
                print("[AR] new_replay=" + new_replay)
            
            clip = VideoFileClip(last_replay)
            duration = clip.duration
            
            if duration > seconds:
                if from_end:
                    if debug_mode: print("[AR] from_end")
                    ffmpeg_extract_subclip(last_replay, duration - seconds, duration+7, targetname=new_replay)
                else:
                    if debug_mode: print("[AR] from_begin")
                    ffmpeg_extract_subclip(last_replay, 0, seconds+7, targetname=new_replay)
            else:
                copyfile(last_replay, new_replay)
                
            clip.reader.close()
            if clip.audio and clip.audio.reader:
                clip.audio.reader.close_proc()
            del clip.reader
            del clip
            
            if remove and os.path.exists(new_replay):
                try:
                    if debug_mode: print("[AR] try remove")
                    for x in range(10):
                        if not file_in_use(last_replay):
                            break
                        if debug_mode: print("[AR] file not writeable, wait 0.5 seconds")
                        sleep(0.5)
                    if debug_mode: print("[AR] delete file:" + last_replay)
                    os.remove(last_replay)
                except:
                    print("[AR] error ", sys.exc_info()[0], " on remove : ", last_replay)

            return duration

def getdata(query):
    try:
        s.send(query)
        data = s.recv(256)
        return data.decode().strip()
    except:
        return None

def ask_livesplit():
    global enabled
    if not enabled:
        return

    global previndex
    global lasttime
    global index
    global splitname
    global curtime
    global totaltime
    global filelist
    global outfile
    global filen
    global oldpb
    global starttime
    global substart
    global oldsubstart
    global recs
    global recfolder
    global dir
    
    status = getdata(getstatus)
    if status != "Running":
        return

    index = getdata(getindex)
    if index:
        try:
            index = int(index)
        except:
            return

    if not recs:
        files_path = os.path.join(recfolder, '*')
        recs = sorted(glob.iglob(files_path), key=os.path.getctime, reverse=True)
        if not recs:
            print("Couldn't find any recordings in '{}'".format(recfolder))
            return

    if previndex != index:
        previndex = index
        index = getdata(getindex)
        
        if index:
            index = int(index)
        
        splitname = getdata(getsplitname)
        
        curtime = getdata(getcurtime)
        if curtime:
            ts, ms = curtime.split('.')
            curtime = sum(int(x) * 60 ** i for i, x in enumerate(reversed(ts.split(':'))))
            curtime = float(f"{curtime}.{ms}")
        else:
            return

        if lasttime:
            sleep(1)
            totaltime = round(curtime - lasttime, 2) + 2
            outfile = "{}_{}_{}.mkv".format(index, totaltime, splitname)
            filelist = glob.glob(dir+"{}_*_*.mkv".format(index))
            if filelist:
                filen = os.path.basename(filelist[0])
                oldpb = float(re.search('.+_(.+)_.+.mkv', filen).group(1))
                print(oldpb)
                print(totaltime)
                if oldpb > totaltime:
                    os.rename(filelist[0], bkpdir+filen)
                    path = dir+"buffer.mkv"
                    if starttime < 0:
                        starttime = 0
                    
                    stime = perf_counter()
                    cmd = [get_setting("FFMPEG_BINARY"), "-y", "-i", recs[0], "-ss", "%0.2f"%starttime, "-c", "copy", dir+"tmp.mkv"]
                    subprocess_call(cmd)
                    duration = cut_replay(totaltime, dir, outfile, True, True, dir+"tmp.mkv")
                    etime = perf_counter()
                    substart = float(f"{etime-stime:0.2f}")
                    if duration:
                        starttime = starttime + duration - 2
                        if substart > 0:
                            starttime = starttime - substart
            else:
                path = dir+"buffer.mkv"
                if starttime < 0:
                    starttime = 0

                stime = perf_counter()
                cmd = [get_setting("FFMPEG_BINARY"), "-y", "-i", recs[0], "-ss", "%0.2f"%starttime, "-c", "copy", dir+"tmp.mkv"]
                subprocess_call(cmd)
                duration = cut_replay(totaltime, dir, outfile, True, True, dir+"tmp.mkv")
                etime = perf_counter()
                substart = float(f"{etime-stime:0.2f}")
                if duration:
                    starttime = starttime + duration - 2
                    if substart > 0:
                        starttime = starttime - substart
    
    else:
        lasttime = getdata(getsplittime)
        print(lasttime)
        if lasttime:
            ts, ms = lasttime.split('.')
            lasttime = sum(int(x) * 60 ** i for i, x in enumerate(reversed(ts.split(':'))))
            lasttime = float(f"{lasttime}.{ms}")

def main_script():
    global debug_mode
    if debug_mode: print("[AR] Updated properties.")
    
    global enabled
    global getindex
    global getsplitname
    global getsplittime
    global getcurtime
    global getcomp
    global getstatus
    global dir
    global bkpdir
    global s
    
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.connect(("localhost", 16834))
    s.settimeout(0.1)

    while(True):
        sleep(0.5)
        ask_livesplit()

main_script()
