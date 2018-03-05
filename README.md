# sharp-sign

Run this with any number of strings as arguments and it will print them inside a frame.
The width of the terminal is considered, strings longer than the with of the
terminal are just split at maximum length.

```
fsharpi sign.fsx lala bubu gugu
```

This on a very narrow terminal will print that:

```
+-----------+
|           |
| lala bubu |
| gugu      |
|           |
+-----------+
```
