# Games Project

![The starting area of the game: a modest, well-lit library, with a locked exit.](/Images/nicelibrary.png)
<span class="caption">The starting area of the game: a modest, well-lit library, with a locked exit.</span>

### Summary

This project was completed for a major module in the third-year of the Computer Science masters course at the University of Bristol. The objective was to work as a team to create a fully-functioning game which utilised novel technology, which for us was Virtual Reality. The project the first two terms of the year: due to the intensity of our first term, our work was effectively split into a planning phase during the first term and an excecution phase during the second term.

Although we began working in a team of six, unfortunately three members left project for various health reasons early in the second term.

### Magical System Design

Our original concept was for an open-ended, systems-driven game in which the key appeal is discovering and experimenting with the game's magical rules, and mastering these systems to solve open-ended challenges. While we were excited about this direction, we quickly learned that the format of assessment would require games to be fully experienced with a 10-minute window, making a redesign necessary. We pivoted to a linear puzzle game, in order to retain the core of the magic systems while making the user experience self-contained and self-evident.

The initial concept saw each spell tied to a unique symbol, which must be drawn to cast the spell, meaning the most significant barrier to progression is player knowledge. However, to better control the difficulty and complexity of puzzles over the course of the game, we designed a system where each spell would be tied to an elemental focus, which must be held for the spell to work. This had the added benefit of allowing symbols to be re-used, corresponding to a different spell for each focus, and also allowed us to design multi-stage puzzles where the intermediate goal is to gain access to a necessary focus. 

![Spellcasting in the library](/Images/fireball.png)
<span class="caption">In this image, we can see the player's right hand has drawn a circle to cast a spell, while their left hand holds a candlestick (a fire focus). Further, a small fireball can be seen hurtling towards a bookcase.</span>

### Implementing Symbol Recognition for spellcasting

To allow the player to cast spells simply by drawing shapes in the air, we used a three-step symbol recognition process.

The first stage is Line Tracking. While holding the drawing button, the system begins sampling 3D positions at the tip of the controller. To ensure the captured points are evenly-spaced, new points are only recorded when the controller tip moves a predefined distance. Also, we perform interpolation when the controller moves further than this distance in a single frame.

This is followed by Plane Fitting, in which all points are projected onto a 2D plane which closely matches the orientation of the drawn symbol. These planes are constructed from three recorded points in the point cloud, which are chosen semi-randomly, but are guarranteed to be distant from eachother. After several iterations of this process, a plane is chosen which best minimises the total distance between to each point from the plane. The handheld controller's orientation is used to determine the orientation of the symbol.

Finally, the processed data are passed into a \$Q gesture recognitizer, which uses nearest-neighbour classification to identify 2D gestures. We used an existing \$Q recogniser in accordance with its license. We made some minor adjustments, including permitting the rejection of a symbol which does not match confidently enough with any symbol; this is because we found players got confused when they accidentally cast spells as they practiced drawing in the air. 

We also allowed the magnitude of the spell to correspond to the size of the symbol, leading to the casting of gargantuan fireballs when players figured this out.

### Traversal and motion sickness

To allow travelling through open spaces, we implemented teleportation as is standard in VR. We were also keen to allow the player to levitate by casting a spell. This is tricky in VR, as virtual movement which does not correspond to physical movement often results in motion sickness. Therefore, our solution was to allow a player to lower and raise themself by crouching and standing on tip-toe respectively, ensuring the virtual change in altitude is at least partially driven by a matching physical movement. This removed or aleviated the issue for a significant majority of players, although we did discover it is surprisingly hard to stand on tip-toe for a sustained period of time.

### Additional screenshots

![A tall room filled with breakable pots, featuring a door with three locks](/Images/pots.png)
![A long, dim room spanned by a broken bridge](/Images/bridge.png)
![An eiree log cabin with an unlit fireplace](/Images/cabin.png)

