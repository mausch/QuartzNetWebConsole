document.addEventListener('DOMContentLoaded', function() {
  const timeToggle = document.getElementById('time-toggle');
  let isUTC = true;
  const originalTimes = new Map();

  function convertTime(timeElement) {
    const originalTime = originalTimes.get(timeElement);
    if (!originalTime) {
      return;
    }

    const date = new Date(Date.parse(originalTime));
    if (isNaN(date.getTime())) {
      return;
    }
    timeElement.textContent = (function() {
      if (isUTC) {
        return date.toISOString();
      } else {
        return date.toLocaleString();
      }
    })();
  }

  function toggleTime() {
    isUTC = !isUTC;
    const timeElements = document.querySelectorAll('.datetime');

    for (const timeElement of timeElements) {
      if (!originalTimes.has(timeElement)) {
        originalTimes.set(timeElement, timeElement.textContent);
      }
      convertTime(timeElement);
    }
  }

  timeToggle.addEventListener('click', toggleTime);
});
