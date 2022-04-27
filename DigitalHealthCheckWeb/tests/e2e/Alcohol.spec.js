describe("Alcohol page", () => {
    const pageToTest = '<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/DoYouDrinkAlcohol';

    beforeEach(async () => {
        await jestPlaywright.resetPage();
        await page.goto(pageToTest);
    })

    it("should show a heading", async () => {
        // after by using expect-playwright
        await expect(page).toMatchText("h1", "Alcohol");
    })

    it("should not go onto next page if no option selected", async () => {
        // Click text=Continue
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/DoYouDrinkAlcohol')).toBeTruthy();
    })

    it("should go to alcohol questions if yes answered ", async () => {
        // Click Yes
        await page.click('#drink-alcohol', 'yes');
        const checked = await page.isChecked('#drink-alcohol', 'yes');
        expect(checked).toBeTruthy();
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/DrinkingFrequency')).toBeTruthy();
        await expect(page).toMatchText("h1", "Alcohol page 2 of 7");
 
    })

    it("should go to GPPAQ if no answered ", async () => {
        // Click No
        await page.click('#drink-alcohol-2', 'no');
        const checked = await page.isChecked('#drink-alcohol-2', 'no');
        expect(checked).toBeTruthy();
        await page.click('text=Continue');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/PhysicalActivity')).toBeTruthy();
        await expect(page).toMatchText("h1", "Physical activity page 1 of 4");
 
    }) 

})