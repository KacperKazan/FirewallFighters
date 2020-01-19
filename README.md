# FirewallFighters
Hack Cambridge 101
## Inspiration
Taking inspiration from the mobile game 'Airport Scanner' we wanted to tie in a fun, easy to learn gameplay along with teaching the 101s of security. Looking at chrome's dinosaur game, we wanted to draw in the simple controls along with continous progression whilst also engaging the user to think about computer security and the types of attacks that a computer might encounter.

## What it does
You play as the 'Firewall Fighter' and you are tasked with scanning incoming data packets for potential threats. You are limited to 3 actions, namely rejecting a packet that you think is malicious (spacebar), speeding up the flow of data (right arrow) or slow down the flow of data (left arrow). 
You can lose the game by letting a malicious packet through or erroneously flagging 3 innocent packets as malicious.
The game progresses by adding more types of malicious attacks to recognise as well as increasing the number of items in a packet with your score increasing whenever you make the correct decision.
If you lose the game by letting a malicious packet through then you get a summary of what the attack was. For example, in the case that the packet contained a virus then the user would get a summary explaining what a virus is etc. In this way we can teach the users what the basic types of computer attacks are.

## How we built it
We build this whole project from scratch in Unity and exported it to a WebGL application. After a long brain storming session we settled on a design and initially split off to work on the user interface and the core game logic. Once we were ready to start merging the two, all the team members rotated flexibly around tasks to polish off the project.

## Challenges we ran into
The biggest challenge that we ran into was the algorithm for randomized placing of items in a packet which would ensure that items don't overlap excessively (making the game unplayable). We finally settled upon a rejection sampling method.

## Accomplishments that we're proud of
As a team, we're really proud of being able to deliver a full game in under 24 hours that fully aligns with our initial design and goals. We belive that the game is simple but engaging and informative with a welcoming retro theme and are very happy with the end product.

## What we've learned
We've all learnt how to create 2D games in Unity including skills in 2D graphic design, animation and also Unity's 2D specific functionality (e.g. using sprites, masks etc...)
Herby learnt how to code in C# and how to use Unity both of which he's never done before.

## What's next for Firewall Fighters
The ultimate goal for this project was to create a game that could be played offline on the Avast Secure Browser. Ideallly we would love to integrate the game into the browser, however failing that we would like to release the game either as a mobile app or a playable, online game.
