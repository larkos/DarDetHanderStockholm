﻿
//GridScrollFx.prototype.options = {

    
//    minDelay: 0,
//    maxDelay: 500,
//    viewportFactor: 0
//}

//GridScrollFx.prototype._init = function () {
//    var self = this, items = [];

//    [].slice.call(this.el.children).forEach(function (el, i) {
//        var item = new GridItem(el);
//        items.push(item);
//    });

//    this.items = items;
//    this.itemsCount = this.items.length;
//    this.itemsRenderedCount = 0;
//    this.didScroll = false;

//    imagesLoaded(this.el, function () {
//        // show grid
//        self.el.style.display = 'block';

//        // initialize masonry
//        new Masonry(self.el, {
//            itemSelector: 'li',
//            isFitWidth: true,
//            transitionDuration: 0
//        });

//        // the items already shown...
//        self.items.forEach(function (item) {
//            if (inViewport(item.el)) {
//                ++self.itemsRenderedCount;
//                classie.add(item.el, 'shown');
//            }
//            else {
//                item.addCurtain();
//                // add random delay
//                item.changeAnimationDelay(Math.random() * (self.options.maxDelay - self.options.minDelay) + self.options.minDelay);
//            }
//        });

//        var onScrollFn = function () {
//            if (!self.didScroll) {
//                self.didScroll = true;
//                setTimeout(function () { self._scrollPage(); }, 200);
//            }

//            if (self.itemsRenderedCount === self.itemsCount) {
//                window.removeEventListener('scroll', onScrollFn, false);
//            }
//        }

//        // animate the items inside the viewport (on scroll)
//        window.addEventListener('scroll', onScrollFn, false);
//        // check if new items are in the viewport after a resize
//        window.addEventListener('resize', function () { self._resizeHandler(); }, false);
//    });
//}

//GridScrollFx.prototype._scrollPage = function () {
//    var self = this;
//    this.items.forEach(function (item) {
//        if (!classie.has(item.el, 'shown') && !classie.has(item.el, 'animate') && inViewport(item.el, self.options.viewportFactor)) {
//            ++self.itemsRenderedCount;

//            if (!item.curtain) {
//                classie.add(item.el, 'shown');
//                return;
//            };

//            classie.add(item.el, 'animate');

//            // after animation ends add class shown
//            var onEndAnimationFn = function (ev) {
//                if (support.animations) {
//                    this.removeEventListener(animEndEventName, onEndAnimationFn);
//                }
//                classie.remove(item.el, 'animate');
//                classie.add(item.el, 'shown');
//            };

//            if (support.animations) {
//                item.curtain.addEventListener(animEndEventName, onEndAnimationFn);
//            }
//            else {
//                onEndAnimationFn();
//            }
//        }
//    });
//    this.didScroll = false;
//}

//GridScrollFx.prototype._resizeHandler = function () {
//    var self = this;
//    function delayed() {
//        self._scrollPage();
//        self.resizeTimeout = null;
//    }
//    if (this.resizeTimeout) {
//        clearTimeout(this.resizeTimeout);
//    }
//    this.resizeTimeout = setTimeout(delayed, 1000);
//}

