describe("Mental wellbeing page", () => {
    const pageToTest = '<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/MentalWellbeing';

    beforeEach(async () => {
        await jestPlaywright.resetPage();
        await page.goto(pageToTest);
    })

    it("should show a heading", async () => {
        // after by using expect-playwright
        await expect(page).toMatchText("h1", "Mental wellbeing");
    })

    it("should not go onto next page if no option selected", async () => {
        // Click text=Continue
        await page.click('button:has-text("Continue")');
        expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/MentalWellbeing')).toBeTruthy();
    })

    it("goes to Complete when all questions answered", async () => {
        await page.click('#anxious', 'notatall');  
        const checked = await page.isChecked('#anxious', 'notatall');
        expect(checked).toBeTruthy();
        await page.click('#control-2', 'severaldays'); 
        await page.click('#feeling-down-2', 'no'); 
        await page.click('#disinterested','yes');
        await page.click('#under-care-2', 'no'); 
        await page.click('button:has-text("Continue")');
        await expect(page.url().startsWith('<digitalHealthCheckURL>/DigitalHealthCheck2_TEST/Complete')).toBeTruthy();
        await expect(page).toMatchText("h1", "Your communication preferences");
 
    }) 

})