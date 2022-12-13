function showScratchcard(onShown) {
    const SCRATCH_TEXT = "Zdrap mnie!";
    const SCRATCH_FONT = "300% Arial";
    const SCRATCH_FILL_COLOR = "#3B3486";
    const SCRATCH_TEXT_COLOR = "#FFE9B1";
    
    const overlay = document.getElementById('scratchie_overlay');
    overlay.style.visibility = 'initial';
    const container = document.getElementById('scratchie');
    container.style.display = 'initial';
    
    const date = getTodayDate();
    let isDrawing = false, lastPoint = null;
    
    const canvas = document.getElementById('scratchie_canvas');
    canvas.style.opacity = '1';
    canvas.style.display = 'initial';
    const rect = canvas.getBoundingClientRect();
    console.log(rect);
    canvas.width = Math.round(rect.right - rect.left);
    canvas.height = Math.round(rect.bottom - rect.top);

    const canvasWidth = canvas.width;
    const canvasHeight = canvas.height;
    const ctx = canvas.getContext('2d');
    const fillRadius = Math.sqrt(canvasWidth * canvasHeight) / 12;
    console.log(canvasWidth, canvasHeight);

    // Initial content
    ctx.beginPath();
    ctx.rect(0, 0, canvasWidth, canvasHeight);
    ctx.fillStyle = SCRATCH_FILL_COLOR;
    ctx.fill();
    ctx.font = SCRATCH_FONT;
    ctx.fillStyle = SCRATCH_TEXT_COLOR;
    ctx.textBaseline = 'middle';
    ctx.textAlign = 'center';
    ctx.fillText(SCRATCH_TEXT, canvasWidth / 2, canvasHeight / 2);

    canvas.addEventListener('mousedown', handleMouseDown);
    canvas.addEventListener('touchstart', handleMouseDown);
    canvas.addEventListener('mousemove', handleMouseMove);
    canvas.addEventListener('touchmove', handleMouseMove);
    canvas.addEventListener('mouseup', handleMouseUp);
    canvas.addEventListener('touchend', handleMouseUp);
    
    function distanceBetween(point1, point2) {
        return Math.sqrt(Math.pow(point2.x - point1.x, 2) + Math.pow(point2.y - point1.y, 2));
    }

    function angleBetween(point1, point2) {
        return Math.atan2( point2.x - point1.x, point2.y - point1.y );
    }

    // Only test every `stride` pixel. `stride`x faster,
    // but might lead to inaccuracy
    function getFilledInPixels(stride) {
        if (!stride || stride < 1) { stride = 1; }

        var pixels   = ctx.getImageData(0, 0, canvasWidth, canvasHeight),
            pdata    = pixels.data,
            l        = pdata.length,
            total    = (l / stride),
            count    = 0;

        // Iterate over all pixels
        for(var i = count = 0; i < l; i += stride) {
            if (parseInt(pdata[i]) === 0) {
                count++;
            }
        }

        return Math.round((count / total) * 100);
    }

    function getMouse(e, canvas) {
        const rect = canvas.getBoundingClientRect();
        console.log(e);
        if (e instanceof MouseEvent) {
            return {x: e.pageX - rect.left + 25, y: e.pageY - rect.top + 25};
        } else if (e instanceof TouchEvent) {
            return {x: e.changedTouches[0].pageX - rect.left + 25, y: e.changedTouches[0].pageY - rect.top + 25};
        }
    }

    function handlePercentage(filledInPixels) {
        filledInPixels = filledInPixels || 0;
        if (filledInPixels > 60) {
            canvas.style.opacity = '0';
            setTimeout(() => canvas.style.display = 'none', 1000);
            overlay.onclick = function () {
                overlay.style.display = 'none';
                container.style.display = 'none';
            }
            onShown();
        }
    }

    function handleMouseDown(e) {
        e.preventDefault();
        isDrawing = true;
        lastPoint = getMouse(e, canvas);
    }

    function handleMouseMove(e) {
        e.preventDefault();
        if (!isDrawing) { return; }
        
        var currentPoint = getMouse(e, canvas),
            dist = distanceBetween(lastPoint, currentPoint),
            angle = angleBetween(lastPoint, currentPoint),
            x, y;

        for (var i = 0; i < dist; i++) {
            x = lastPoint.x + (Math.sin(angle) * i) - 25;
            y = lastPoint.y + (Math.cos(angle) * i) - 25;
            console.log(x, y);
            ctx.globalCompositeOperation = 'destination-out';
            ctx.fillStyle = "#000000";
            ctx.beginPath();
            ctx.arc(x, y, fillRadius, 0, 2 * Math.PI);
            ctx.fill();
        }

        lastPoint = currentPoint;
        handlePercentage(getFilledInPixels(32));
    }

    function handleMouseUp(e) {
        e.preventDefault();
        isDrawing = false;
    }
}