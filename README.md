# Dillinger
# Unity_Unit_ScrollPage

目前只有Horizontal的部分，Vertical其實原理一樣，之後有空補上

# 使用說明:

使用UGUI最基本的ScrollView架構作調整:

  - ScrollView: 一個含有scrollview元件的物件，也就是這次ScrollControl_horizontal.cs要放置的物件
      - Viewport: 主要是使用mask遮罩，並不影響整體
          - Content: 元素(element)的父物件，所有元素都掛在他下面，同時請務必要加上GridLayoutGroup這個排版元件
              - Element: 各個元素，可依需求增加按鈕等其他UI元件
      - Butten_Next/Prev: 跳至上/下頁的按鈕，掛在ScrollView下即可(非必須)
      - Scrollbar: 額外一種控制手段(非必須)
      - Paginations: 用來顯示目前頁數/總頁數，toggle的父物件，要使用時需將此拖給ScrollControl_horizontal.cs(非必須)
      -Toggle: 利用UGUI的toggle進行顯示頁數控制
      
以上為在Unity中Hierarchy架構

其他沒提到的東西就是修飾用...:P


# 注意事項:

  - 目前Toggle不會隨著頁面數作動態增減
  - 如需使用Scrollbar會需要EventListener.cs作為事件監聽(不用綁至物件上)
  - 在ScrollView的ScrollView元件中，請將Inertia取消勾選
  
# 備註:
  這次是參考以下兩位神人的做法，之後除了將Vertical更新上去外，也會盡量完善其他部分!

  - [Simon Jackson]

  - [雨松MOMO]

[Simon Jackson]: <https://bitbucket.org/ddreaper/unity-ui-extensions/overview>
[雨松MOMO]: <http://www.xuanyusong.com/archives/3325>
