Pack-O-Tron
===========

[Challenge #115 [Difficult] Pack-o-Tron 5000](http://www.reddit.com/r/dailyprogrammer/comments/15uohz/122013_challenge_115_difficult_packotron_5000/)


Input

    4 4
    4 2
    8 3
    7 1
    6 6
    2 3
    3 2
    4 3

Generates this 11x11 map with 95% efficiency.

    1  1  1  .  1  1  1  1  1  1  1 
    1  1  1  0  0  0  0  0  0  2  2 
    1  1  1  0  0  0  0  0  0  2  2 
    1  1  1  0  0  0  0  0  0  2  2 
    1  1  1  0  0  0  0  0  0  6  6 
    1  1  1  0  0  0  0  0  0  6  6 
    1  1  1  0  0  0  0  0  0  6  6 
    1  1  1  .  9  9  9  9  6  6  . 
    5  5  5  5  9  9  9  9  6  6  . 
    5  5  5  5  9  9  9  9  6  6  . 
    5  5  5  5  9  9  9  9  6  6  .
    
Using [this input](http://pastebin.com/sxk6sA4U) from /u/Cosmologicon  [this output](https://gist.github.com/4466497) is created in 200ms using a 206x206 grid (93,6% efficiency).
    
