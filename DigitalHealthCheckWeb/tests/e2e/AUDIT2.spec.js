describe("AUDIT2 page", () => {
    const pageToTest = '<digitalHealthCheckURL>/DigitalHealthCheck2_Test/AUDIT2';

    beforeEach(async () => {
        await jestPlaywright.resetPage();
        await page.goto(pageToTest);
    })

    it("should show a heading", async () => {
        // after by using expect-playwright
        await expect(page).toMatchText("h1", "Alcohol page 4 of 7");
    })

    it("should not go onto next page if no option selected", async () => {
        // Click text=Continue
        await page.click('text=Continue');
        //expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/AUDIT2')).toBeTruthy();
        await expect(page).toMatchText("h1", "Alcohol page 4 of 7");
    })

    it("should not go onto next page if only one option selected", async () => {
        await page.click('#failed-responsibility-2');
        const checked = await page.isChecked('#failed-responsibility-2', 'lessThanMonthly');
        expect(checked).toBeTruthy();
        // Click text=Continue
        await page.click('text=Continue');
        //expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/AUDIT2')).toBeTruthy();
        await expect(page).toMatchText("h1", "Alcohol page 4 of 7");
    })

    it("goes to AUDIT3 if both questions selected ", async () => {
        await page.click('#unable-to-stop-2', 'lessThanMonthly');
        await page.click('#failed-responsibility-2');
        const checked = await page.isChecked('#failed-responsibility-2', 'lessThanMonthly');
        expect(checked).toBeTruthy();
        await page.click('text=Continue');
        await page.waitForSelector ("h1", "Alcohol page 5 of 7");
        //expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/AUDIT3')).toBeTruthy();
        await expect(page).toMatchText("h1", "Alcohol page 5 of 7");
     }) 


})