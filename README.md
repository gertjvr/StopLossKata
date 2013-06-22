Stop Loss Kata
========================
<br>

This is an attempt to answer this challenge https://gist.github.com/michaelnoonan/5566257
-
<br>

Stop Loss Kata (original source: https://gist.github.com/gregoryyoung/1500720)
Developing solutions when time is involved is tricky, but testing when time is involved is yet another problem.

<br>

A trailing stop loss is a term used in financial trading. For a very in depth explanation you can read here (http://www.investopedia.com/articles/trading/03/080603.asp) and here (http://en.wikipedia.org/wiki/Order_(exchange)#Stop_orders), however we do not need a huge amount of background in order to do the kata as we are going to limit the problem a bit.

<br>

Say you buy into a stock at $10. You want it to automatically get sold if the stock goes below $9 to limit your exposure. This threshold is set by subtracting 10% from the original position. The term "trailing" means that if the price goes up to $11 then the sell point becomes $10, if it goes up to $15 then the sell point becomes $14, maintaining the original 10% margin of $1.
 
<br>
 
The kata is to create something that implements a trailing stop loss and to do it with TDD. To make matters more fun the sell point should only move up if it’s held for more than 15 seconds and the stop loss should only be triggered if the price point is held below the sell point for more than 30 seconds.
 
<br>

To begin the kata, implement a “PositionAcquired” message that your code will receive when a stock position is acquired, and a "PriceChanged" message that your code will receive every time the price changes. (Just implement them as methods that receive each message, assuming later you will hook it up into something that provides a data feed).
<br>
Writing the tests should be fun.
-
<br>
Some goals/hints:
-
  - The implementation should be a simple set of C# classes that illustrate the single responsibility principle
  - You shouldn’t need any external resources like persistent storage etc, just consider it as an in-proc, in-memory problem
  - Tests shouldn’t use Thread.Sleep()
  - Can you do it without holding any state except for an integer variable? What trade-offs would you have to make?