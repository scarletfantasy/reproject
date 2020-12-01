# reproject
a unity proj implement the reproject in vr

* using forward warpping to generate the target image from the original imgae
* using ray tracing to implement the hole filling step

just small modify the render order for adding the dx10 ray tracing shader,the ray tracing step not modify the render pipeline(e.g. srp)

# result and analyze
platform: gtx1060,i7-7700hq


![avatar](屏幕截图%202020-11-17%20111635.png)

|render step|render consuming|
|---|---|
|forwardrendering and depth| 1.03ms|
|reproject|0.25ms|
|hole filling with ray tracing|1.74ms|


|render step|render consuming|
|---|---|
|forwardrendering and depth| 1.03ms|
|reproject|0.11ms|
|rerender hole filling |0.82ms|

it's obvious that the reproject step reduce the time consuming of rerendering ,but the ray tracing step is not satisfying(i guess it's due to my hardware limit). 

