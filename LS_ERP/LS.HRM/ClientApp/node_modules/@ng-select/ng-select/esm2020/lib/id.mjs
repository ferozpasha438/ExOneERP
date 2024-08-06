export function newId() {
    // First character is an 'a', it's good practice to tag id to begin with a letter
    return 'axxxxxxxxxxx'.replace(/[x]/g, () => {
        // eslint-disable-next-line no-bitwise
        const val = Math.random() * 16 | 0;
        return val.toString(16);
    });
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoiaWQuanMiLCJzb3VyY2VSb290IjoiIiwic291cmNlcyI6WyIuLi8uLi8uLi8uLi9zcmMvbmctc2VsZWN0L2xpYi9pZC50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiQUFBQSxNQUFNLFVBQVUsS0FBSztJQUNqQixpRkFBaUY7SUFDakYsT0FBTyxjQUFjLENBQUMsT0FBTyxDQUFDLE1BQU0sRUFBRSxHQUFHLEVBQUU7UUFDdkMsc0NBQXNDO1FBQ3RDLE1BQU0sR0FBRyxHQUFHLElBQUksQ0FBQyxNQUFNLEVBQUUsR0FBRyxFQUFFLEdBQUcsQ0FBQyxDQUFDO1FBQ25DLE9BQU8sR0FBRyxDQUFDLFFBQVEsQ0FBQyxFQUFFLENBQUMsQ0FBQztJQUM1QixDQUFDLENBQUMsQ0FBQztBQUNQLENBQUMiLCJzb3VyY2VzQ29udGVudCI6WyJleHBvcnQgZnVuY3Rpb24gbmV3SWQoKSB7XG4gICAgLy8gRmlyc3QgY2hhcmFjdGVyIGlzIGFuICdhJywgaXQncyBnb29kIHByYWN0aWNlIHRvIHRhZyBpZCB0byBiZWdpbiB3aXRoIGEgbGV0dGVyXG4gICAgcmV0dXJuICdheHh4eHh4eHh4eHgnLnJlcGxhY2UoL1t4XS9nLCAoKSA9PiB7XG4gICAgICAgIC8vIGVzbGludC1kaXNhYmxlLW5leHQtbGluZSBuby1iaXR3aXNlXG4gICAgICAgIGNvbnN0IHZhbCA9IE1hdGgucmFuZG9tKCkgKiAxNiB8IDA7XG4gICAgICAgIHJldHVybiB2YWwudG9TdHJpbmcoMTYpO1xuICAgIH0pO1xufVxuIl19