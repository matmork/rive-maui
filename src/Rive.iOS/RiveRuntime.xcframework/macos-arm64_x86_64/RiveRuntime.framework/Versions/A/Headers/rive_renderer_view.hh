#import <MetalKit/MetalKit.h>
#import <RiveRuntime/RiveArtboard.h>
#import <RiveRuntime/Rive.h>

#import <Metal/Metal.h>

NS_ASSUME_NONNULL_BEGIN

@interface RiveRendererView : MTKView

- (instancetype)initWithFrame:(CGRect)frameRect;
- (void)alignWithRect:(CGRect)rect
          contentRect:(CGRect)contentRect
            alignment:(RiveAlignment)alignment
                  fit:(RiveFit)fit;
- (void)save;
- (void)restore;
- (void)transform:(float)xx xy:(float)xy yx:(float)yx yy:(float)yy tx:(float)tx ty:(float)ty;
- (void)drawWithArtboard:(RiveArtboard*)artboard;
- (void)drawRive:(CGRect)rect size:(CGSize)size;
- (void)drawInRect:(CGRect)rect withCompletion:(_Nullable MTLCommandBufferHandler)completionHandler;
- (bool)isPaused;
- (CGPoint)artboardLocationFromTouchLocation:(CGPoint)touchLocation
                                  inArtboard:(CGRect)artboardRect
                                         fit:(RiveFit)fit
                                   alignment:(RiveAlignment)alignment;

NS_ASSUME_NONNULL_END
@end
