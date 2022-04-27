describe("AUDIT3 page", () => {
    const pageToTest = '<digitalHealthCheckURL>/DigitalHealthCheck2_Test/AUDIT3';

    beforeEach(async () => {
        await jestPlaywright.resetPage();
        await page.goto(pageToTest);
    })

    it("should show a heading", async () => {
        // after by using expect-playwright
        await expect(page).toMatchText("h1", "Alcohol page 5 of 7");
    })

    it("should not go onto next page if no option selected", async () => {
        // Click text=Continue
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_Test/AUDIT3')).toBeTruthy();
    })

    it("should not go onto next page if only first option selected", async () => {
        await page.click('#guilt');
        const checked = await page.isChecked('#guilt', 'never');
        expect(checked).toBeTruthy();
        // Click text=Continue
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_Test/AUDIT3')).toBeTruthy();
    })

    it("goes to AUDIT4 if both questions selected ", async () => {
        // Click #Guilt-3
        await page.click('#guilt-3');
        const checked = await page.isChecked('#guilt-3', 'monthly');
        expect(checked).toBeTruthy();
        await page.click('#morning-after-2');
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_Test/AUDIT4')).toBeTruthy();
        await expect(page).toMatchText("h1", "Alcohol page 6 of 7");
 
    }) 

})