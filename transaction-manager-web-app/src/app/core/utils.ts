export class Utils {
    public static API_BASE_URL = 'https://localhost:44353/api/';

    public static printValueWithHeaderToConsole(header: string, value: any): void {
        console.log('====================================================');
        console.log(header);
        console.log('====================================================');
        console.dir(value);
        console.log('-----------------------------------------------------');
    }
}
