describe("AUDIT4 page", () => {
    const pageToTest = '<digitalHealthCheckURL>/DigitalHealthCheck2_Test/AUDIT4';

    beforeEach(async () => {
        await jestPlaywright.resetPage();
        await page.goto(pageToTest);
    })

    it("should show a heading", async () => {
        // after by using expect-playwright
        await expect(page).toMatchText("h1", "Alcohol page 6 of 7");
    })

    it("should not go onto next page if no option selected", async () => {
        // Click text=Continue
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_Test/AUDIT4')).toBeTruthy();
    })

    it("should not go onto next page if only first option selected", async () => {
        await page.click('#injured');
        const checked = await page.isChecked('#injured', 'notInLastYear');
        expect(checked).toBeTruthy();
        // Click text=Continue
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_Test/AUDIT4')).toBeTruthy();
    })

    it("goes to Physical Activity if both questions selected ", async () => {
        await page.click('#injured');
        const checked = await page.isChecked('#injured', 'notInLastYear');
        expect(checked).toBeTruthy();
        await page.click('#memory-loss-2');
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_Test/AUDIT5')).toBeTruthy();
        await expect(page).toMatchText("h1", "Alcohol page 7 of 7");
 
    }) 

})