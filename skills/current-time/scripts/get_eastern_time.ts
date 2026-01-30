#!/usr/bin/env ts-node

/**
 * Get Current Date and Time in Eastern Time Zone
 * 
 * This script retrieves the current date and time and formats it for the Eastern Time Zone (ET).
 * It handles both EST (Eastern Standard Time) and EDT (Eastern Daylight Time) automatically.
 */

interface TimeInfo {
  dateTime: string;
  isDST: boolean;
  isoString: string;
  unixTimestamp: number;
}

/**
 * Gets the current date and time in Eastern Time Zone
 * @returns TimeInfo object with formatted date/time information
 */
function getEasternTime(): TimeInfo {
  // Create a date object for the current time
  const now = new Date();
  
  // Format for Eastern Time Zone
  const options: Intl.DateTimeFormatOptions = {
    timeZone: 'America/New_York',
    year: 'numeric',
    month: '2-digit',
    day: '2-digit',
    hour: '2-digit',
    minute: '2-digit',
    second: '2-digit',
    hour12: false,
    timeZoneName: 'short'
  };
  
  const formatter = new Intl.DateTimeFormat('en-US', options);
  const parts = formatter.formatToParts(now);
  
  // Extract parts
  const partsMap = new Map(parts.map(p => [p.type, p.value]));
  
  const year = partsMap.get('year') || '';
  const month = partsMap.get('month') || '';
  const day = partsMap.get('day') || '';
  const hour = partsMap.get('hour') || '';
  const minute = partsMap.get('minute') || '';
  const second = partsMap.get('second') || '';
  const timeZoneName = partsMap.get('timeZoneName') || '';
  
  // Determine if DST is in effect (EDT vs EST)
  const isDST = timeZoneName === 'EDT';
  
  // Format outputs
  const date = `${year}-${month}-${day}`;
  const time = `${hour}:${minute}:${second}`;
  const dateTime = `${date} ${time} ${timeZoneName}`;
  
  return {
    dateTime,
    date,
    time,
    timezone: timeZoneName,
    isDST,
    isoString: now.toISOString(),
    unixTimestamp: Math.floor(now.getTime() / 1000)
  };
}

/**
 * Main execution
 */
function main(): void {
  const timeInfo = getEasternTime();
  
  // Output as formatted text
  console.log('Current Eastern Time:');
  console.log(`Date & Time: ${timeInfo.dateTime}`);
  console.log(`DST Active:  ${timeInfo.isDST ? 'Yes (EDT)' : 'No (EST)'}`);
  console.log(`ISO 8601:    ${timeInfo.isoString}`);
  console.log(`Unix Time:   ${timeInfo.unixTimestamp}`);
  
  //console.log(JSON.stringify(timeInfo, null, 2));
}

// Run if executed directly
if (require.main === module) {
  main();
}

// Export for use as a module
export { getEasternTime, TimeInfo };